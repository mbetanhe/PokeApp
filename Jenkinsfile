pipeline {
    agent any

    environment {
        PROJECT_PATH = 'C:\\Jenkins\\workspace\\PokeApp\\PokeApp.Presentation.API'  // Ruta del workspace
        CSPROJ_PATH = 'C:\\Jenkins\\workspace\\PokeApp\\PokeApp.Presentation.API\\MiWebApp.csproj'
        OUTPUT_PATH = 'C:\\inetpub\\wwwroot\\General\\api_test_cicd'
        BACKUP_ROOT = 'C:\\Users\\mbetanhe\\Documents\\Backups\\Produccion'
        APP_POOL_NAME = 'api_test_cicd'
    }

    stages {

        stage('Detener App Pool') {
            steps {
                powershell '''
                    Import-Module WebAdministration
                    Write-Output "Deteniendo App Pool: $env:APP_POOL_NAME"
                    Stop-WebAppPool -Name "$env:APP_POOL_NAME"
                '''
            }
        }

        stage('Backup antes de publicar') {
            steps {
                powershell '''
                    $timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
                    $backupPath = "$env:BACKUP_ROOT\\$timestamp"

                    Write-Output "Creando backup en: $backupPath"
                    New-Item -ItemType Directory -Path $backupPath -Force | Out-Null
                    Copy-Item -Path "$env:OUTPUT_PATH\\*" -Destination $backupPath -Recurse -Force
                '''
            }
        }

        stage('Restaurar') {
            steps {
                bat "dotnet restore \"$env:CSPROJ_PATH\""
            }
        }

        stage('Compilar') {
            steps {
                bat "dotnet build \"$env:CSPROJ_PATH\" --configuration Release"
            }
        }

        stage('Publicar') {
            steps {
                bat """
                    dotnet publish \"$env:CSPROJ_PATH\" ^
                        --configuration Release ^
                        --framework net8.0 ^
                        --runtime win-x64 ^
                        --self-contained true ^
                        --output \"$env:OUTPUT_PATH\" ^
                        /p:PublishTrimmed=false
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
            echo 'El despliegue falló. Verifica los logs.'
        }
        success {
            echo 'Despliegue completado exitosamente.'
        }
    }
}
