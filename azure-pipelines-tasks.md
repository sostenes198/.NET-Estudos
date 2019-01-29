


            // Task com exemplo chamada rest powerhsell e seta valor nas variáveis de ambiente do agente
          - task: PowerShell@2
            inputs:
              targetType: 'inline'
              script: |
                # Write your PowerShell commands here.                 
                
                $headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
                $headers.Add("head-1", "1")
                
                $response = Invoke-RestMethod 'https://localhost:5003/test' -Method 'GET' -Headers $headers
                $response | ConvertTo-Json      

                $response.testKey = 'asdasdsdas'                        
                
                $response.PSObject.Properties | ForEach-Object{
                  if([string]::IsNullOrEmpty($_.Value))
                  {
                      Write-Error ('Variavel ' + $_.Name + ' nao pode ser nulla ou vazia.')
                      exit 1
                  }
                  
                  Write-Host ("##vso[task.setvariable variable=" + $_.Name + ";]" + $_.Value)
                }              


            // Task de exemplo para fazer replace via powershell
          - task: PowerShell@2
            displayName: "Transform YAML FILE"
            inputs:
              targetType: "inline"
              script: |
                # Write your PowerShell commands here.                
                ((Get-Content .\test.yaml -Raw) -replace 'test','$(testKey)' | Set-Content .\config.yaml

            // Display file com powershell
          - task: PowerShell@2
            displayName: 'Display YAML FILE AFTER TRANSFORM'
            inputs:
              targetType: 'inline'
              script: |
                # Write your PowerShell commands here.
                Get-Content .\config.yaml