pipeline {
    
    agent none

    environment {
        COMPOSE_PROJECT_NAME = "${env.JOB_NAME}-${env.BUILD_ID}"
    }
    stages {
      
        stage('Build') {

            agent {
                dockerfile {
                    // alwaysPull false
                    // image 'microsoft/dotnet:2.1-sdk'
                    // reuseNode false
                    args '-u root:root'
                }
            }

            steps {

                echo sh(script: 'env|sort', returnStdout: true)

                sh 'dotnet build ./Oragon.Spring.Vault.sln'

            }

        }

	 
        stage('Test') {

            agent {
                dockerfile {
                    args '-u root:root -v /var/run/docker.sock:/var/run/docker.sock'
                }
            }

            when { not {buildingTag() } }

            steps {

                withCredentials([usernamePassword(credentialsId: 'SonarQube', passwordVariable: 'SONARQUBE_KEY', usernameVariable: 'DUMMY' )]) {

                    sh  '''

                        export PATH="$PATH:/root/.dotnet/tools"

                        dotnet test ./Oragon.Spring.Vault.Tests/Oragon.Spring.Vault.Tests.csproj \
                            --configuration Debug \
                            --output ../output-tests  \
                            /p:CollectCoverage=true \
                            /p:CoverletOutputFormat=opencover \
                            /p:CoverletOutput='/output-coverage/coverage.xml' \
                            /p:Exclude="[Oragon.*.Tests]*"

                        dotnet sonarscanner begin /k:"oragon-spring-vault" \
                            /d:sonar.host.url="http://sonar.oragon.io" \
                            /d:sonar.login="$SONARQUBE_KEY" \
                            /d:sonar.cs.opencover.reportsPaths="/output-coverage/coverage.xml" \
                            /d:sonar.coverage.exclusions="Oragon.Spring.Vault.Tests/**/*,tests/**/*,Examples/**/*,**/*.CodeGen.cs" \
                                /d:sonar.test.exclusions="Oragon.Spring.Vault.Tests/**/*,tests/**/*,Examples/**/*,**/*.CodeGen.cs" \
                                     /d:sonar.exclusions="Oragon.Spring.Vault.Tests/**/*,tests/**/*,Examples/**/*,**/*.CodeGen.cs"
                        
                        dotnet build ./Oragon.Spring.Vault.sln
                        dotnet sonarscanner end /d:sonar.login="$SONARQUBE_KEY"
                        '''

                }

            }

        }

        stage('Pack') {

            agent {
                dockerfile {
                    args '-u root:root'
                }
            }

            when { buildingTag() }

            steps {

                script{

                    def projetcs = [
                        './Oragon.Spring.Vault/Oragon.Spring.Vault.csproj'
                    ]

                    if (env.BRANCH_NAME.endsWith("-alpha")) {

                        for (int i = 0; i < projetcs.size(); ++i) {
                            sh "dotnet pack ${projetcs[i]} --configuration Debug /p:PackageVersion=${BRANCH_NAME} --include-source --include-symbols --output ../output-packages"
                        }

                    } else if (env.BRANCH_NAME.endsWith("-beta")) {

                        for (int i = 0; i < projetcs.size(); ++i) {
                            sh "dotnet pack ${projetcs[i]} --configuration Release /p:PackageVersion=${BRANCH_NAME} --output ../output-packages"                        
                        }

                    } else {

                        for (int i = 0; i < projetcs.size(); ++i) {
                            sh "dotnet pack ${projetcs[i]} --configuration Release /p:PackageVersion=${BRANCH_NAME} --output ../output-packages"                        
                        }

                    }

                }

            }

        }

        stage('Publish') {

            agent {
                dockerfile {
                    args '-u root:root'
                }
            }

            when { buildingTag() }

            steps {
                
                script {
                    
                    def publishOnNuGet = ( env.BRANCH_NAME.endsWith("-alpha") == false );

                    withCredentials([usernamePassword(credentialsId: 'myget-oragon', passwordVariable: 'MYGET_KEY', usernameVariable: 'DUMMY' )]) {
                        
                        sh 'for pkg in ./output-packages/*.nupkg ; do dotnet nuget push "$pkg" -k "$MYGET_KEY" -s https://www.myget.org/F/oragon/api/v3/index.json -ss https://www.myget.org/F/oragon/symbols/api/v2/package ; done'

                    }

                    if (publishOnNuGet) {
                        
                        withCredentials([usernamePassword(credentialsId: 'nuget-luizcarlosfaria', passwordVariable: 'NUGET_KEY', usernameVariable: 'DUMMY')]) {

                            sh 'for pkg in ./output-packages/*.nupkg ; do dotnet nuget push "$pkg" -k "$NUGET_KEY" -s https://api.nuget.org/v3/index.json ; done'

                        }

                    }                    
                }
            }
        }
    }
    post {

        always {
            node('master'){
                
                sh  '''
                
               

                '''


            }
        }
    }
}