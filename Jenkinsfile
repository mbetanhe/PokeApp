pipeline {
  agent any

  environment {
    DOTNET       = 'C:\\Program Files\\dotnet\\dotnet.exe'
    CSPROJ_PATH  = 'PokeApp.Presentation.API\\PokeApp.Presentation.API.csproj'

    // Publicación
    OUTPUT_PATH  = 'C:\\inetpub\\wwwroot\\General\\api_test_cicd'
    STAGING_PATH = '${WORKSPACE}\\artifacts'

    // IIS
    APP_POOL_NAME = 'api_test_cicd'

    // Backups
    BACKUP_ROOT = 'C:\\Users\\mbetanhe\\Documents\\Backups\\Produccion'
  }

  stages {
    stage('Checkout') {
      steps { checkout scm }
    }

    stage('Sanity .NET') {
      steps {
        bat 'where dotnet'
        bat '"%DOTNET%" --info'
        bat '"%DOTNET%" --list-sdks'
        bat 'dir "%WORKSPACE%\\PokeApp.Presentation.API"'
      }
    }

    stage('Detener App Pool') {
      steps {
        powershell '''
          Import-Module WebAdministration
          Write-Output "Deteniendo App Pool: $env:APP_POOL_NAME"
          Stop-WebAppPool -Name "$env:APP_POOL_NAME" -ErrorAction SilentlyContinue
        '''
      }
    }

stage('Backup antes de publicar') {
  steps {
    powershell '''
      $ts   = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
      $dest = Join-Path $env:BACKUP_ROOT $ts
      New-Item -ItemType Directory -Force -Path $dest | Out-Null

      if (Test-Path $env:OUTPUT_PATH) {
        Write-Output "Copiando backup a $dest"
        robocopy $env:OUTPUT_PATH $dest * /MIR /R:1 /W:1 /NFL /NDL /NJH /NJS
        $code = $LASTEXITCODE
        # 0 = no copy; 1 = files copied; 2 = extra files; 3 = 1+2 … todos son éxito.
        if ($code -le 3) { exit 0 } else { Write-Error "RoboCopy failed with exit code $code"; exit $code }
      } else {
        Write-Output "OUTPUT_PATH no existe: $env:OUTPUT_PATH"
      }
    '''
  }
}


    stage('Restore') {
      steps {
        bat '"%DOTNET%" restore "%CSPROJ_PATH%"'
      }
    }

    stage('Build') {
      steps {
        bat '"%DOTNET%" build "%CSPROJ_PATH%" -c Release --no-restore'
      }
    }

    stage('Publish (framework-dependent)') {
      steps {
        // Sin --runtime y sin --self-contained
        bat """
          if not exist "%STAGING_PATH%" mkdir "%STAGING_PATH%"
          "%DOTNET%" publish "%CSPROJ_PATH%" ^
            -c Release ^
            -f net8.0 ^
            -o "%STAGING_PATH%" ^
            /p:PublishTrimmed=false ^
            --no-build
        """
      }
    }

    stage('Deploy a IIS') {
      steps {
        powershell '''
          if (!(Test-Path $env:OUTPUT_PATH)) { New-Item -ItemType Directory -Force -Path $env:OUTPUT_PATH | Out-Null }
          Write-Output "Sincronizando a $env:OUTPUT_PATH"
          robocopy $env:STAGING_PATH $env:OUTPUT_PATH * /MIR /R:1 /W:1 | Out-Null
        '''
      }
    }

    stage('Iniciar App Pool') {
      steps {
        powershell '''
          Import-Module WebAdministration
          Write-Output "Iniciando App Pool: $env:APP_POOL_NAME"
          Start-WebAppPool -Name "$env:APP_POOL_NAME"
        '''
      }
    }
  }

  post {
    success { echo 'Despliegue completado exitosamente.' }
    failure { echo 'El despliegue falló. Revisa Sanity .NET y Publish.' }
  }
}
