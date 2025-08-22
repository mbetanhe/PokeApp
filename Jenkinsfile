pipeline {
  agent any

  environment {
    DOTNET       = 'C:\\Program Files\\dotnet\\dotnet.exe'
    CSPROJ_PATH  = 'PokeApp.Presentation.API\\PokeApp.Presentation.API.csproj'

    // carpetas
    OUTPUT_PATH  = 'C:\\inetpub\\wwwroot\\General\\api_test_cicd'
    STAGING_REL  = 'artifacts'   // relativa al WORKSPACE

    // IIS y backups
    APP_POOL_NAME = 'api_test_cicd'
    BACKUP_ROOT   = 'C:\\Users\\mbetanhe\\Documents\\Backups\\Produccion'
  }

  stages {
    stage('Checkout') { steps { checkout scm } }

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
        bat """
          if not exist "%WORKSPACE%\\%STAGING_REL%" mkdir "%WORKSPACE%\\%STAGING_REL%"
          "%DOTNET%" publish "%CSPROJ_PATH%" ^
            -c Release ^
            -f net8.0 ^
            -o "%WORKSPACE%\\%STAGING_REL%" ^
            /p:PublishTrimmed=false ^
            --no-build
        """
      }
    }

    stage('Deploy a IIS') {
      steps {
        powershell '''
          $src = Join-Path $env:WORKSPACE $env:STAGING_REL
          $dst = $env:OUTPUT_PATH
          if (!(Test-Path $dst)) { New-Item -ItemType Directory -Force -Path $dst | Out-Null }
          Write-Output "Sincronizando de $src a $dst"
          robocopy $src $dst * /MIR /R:1 /W:1 /NFL /NDL /NJH /NJS
          $code = $LASTEXITCODE
          if ($code -le 3) { exit 0 } else { Write-Error "RoboCopy failed with exit code $code"; exit $code }
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
