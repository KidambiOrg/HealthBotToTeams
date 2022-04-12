# HealthBotToTeams
Post messages to Teams via HealthBot and Power Automate

## Application Architecture
![Health Bot To Teams](applicationarchitecture.png)

## Pre-Requisites
1. Need permission to install apps in Teams
2. Need permission to create resources in Azure
   1. Health Bot
   2. Storage Account
   3. Azure Functions
3. Need Power Apps and Power Automate licenses

## Function Keys for Azure Function
The Azure function is responsible for generating the Health Bot JWT token. It leverages, by default, Function Key. Configure [Function Keys](https://docs.microsoft.com/en-us/azure/azure-functions/security-concepts?tabs=v4#secure-operation) after deployment.

## Register Health Bot in Teams
1. Create HealthBot in Azure
2. Open Health bot designer
3. From side meny, click Integration/Channels
4. Enable `Microsoft Teams` and click view.
5. Copy the `Bot Id`
6. Download the `manifest.zip` file and extract it to a temp folder.
7. Open `manifest.json` in a text editor and replace the values for `id` and `botId` with the value from step 5.
8. Zip up the contents of the folder into `manifest.json`
9. Go to Teams and deploy the bot to Teams
10. In Teams, find the bot by name or id and initiate conversation by saying `Hello`