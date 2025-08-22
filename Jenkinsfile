pipeline {
  agent any

  environment {
    // Fuerza el host x64 (evita depender del PATH del servicio)
    DOTNET      = 'C:\\Program Files\\dotnet\\dotnet.exe'

    // RUTA CORRECTA: tu repo no tiene 'src\\'
    CSPROJ_PATH = 'PokeApp.Presentation.API\\PokeApp.Presentation.API.csproj'

    OUTPUT_PATH = 'C:\\inetpub\\wwwroot\\General\\api_test_cicd'
    BACKUP_ROOT = 'C:\\Users\\mbetanhe\\Documents\\Backups\\Produccion'
    APP_POOL_NAME = 'api_test_cicd'
  }

  stages {

    stage('Checkout') {
      steps {
        checkout scm
      }
    }

    stage('Sanity .NET') {
      steps {
        bat 'where dotnet'
        bat "\"%DOTNET%\" --info"
        bat "\"%DOTNET%\" --list-sdks"
        // Confirma que el .csproj existe
        bat "dir \"%WORKSPACE%\\PokeApp.Presentation.API\""
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
          $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
          $backupPath = Join-Path $env:BACKUP_ROOT $timestamp
          Write-Output "Creando backup en: $backupPath"
          New-Item -ItemType Directory -Path $backupPath -Force | Out-Null
          if (Test-Path $env:OUTPUT_PATH) {
            Copy-Item -Path (Join-Path $env:OUTPUT_PATH '*') -Destination $backupPath -Recurse -Force
          } else {
            Write-Output "OUTPUT_PATH no existe: $env:OUTPUT_PATH"
          }
        '''
      }
    }

    stage('Restaurar') {
      steps {
        bat "\"%DOTNET%\" restore \"${env.CSPROJ_PATH}\""
      }
    }

    stage('Compilar') {
      steps {
        bat "\"%DOTNET%\" build \"${env.CSPROJ_PATH}\" --configuration Release --no-restore"
      }
    }

    stage('Publicar') {
      steps {
        // Publica directo al sitio (ya detuviste el App Pool). Si prefieres, publica a una carpeta temp y luego copia.
        bat """
          \"%DOTNET%\" publish \"${env.CSPROJ_PATH}\" ^
            --configuration Release ^
            --framework net8.0 ^
            --runtime win-x64 ^
            --self-contained true ^
            --output \"${env.OUTPUT_PATH}\" ^
            /p:PublishTrimmed=false ^
            --no-build
        """
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
    failure {
      echo 'El despliegue falló. Verifica los logs (especialmente Sanity .NET).'
    }
    success {
      echo 'Despliegue completado exitosamente.'
    }
  }
}
