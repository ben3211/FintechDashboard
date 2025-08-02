# FintechDashboard
<img width="1907" height="917" alt="FinTech_Dashboard_GitHub" src="https://github.com/user-attachments/assets/de4176b1-0832-47b2-8b8b-f7f72b3d202c" />

## API Key Setup (Environment Variables)

This project requires several API keys for external services. **Do NOT put your real API keys in code or commit them to git!**

### Required Environment Variables

| Variable Name           | Description                |
|------------------------|----------------------------|
| ALPHAVANTAGE_API_KEY   | Your AlphaVantage API key  |
| NEWSAPI_API_KEY        | Your NewsAPI.org API key   |
| OPENAI_API_KEY         | Your OpenAI API key        |

### Local Development (launchSettings.json)

For convenience, a template file is provided: `FintechDashboard.Api/Properties/launchSettings.Template.json`.

**After cloning the repo:**
1. Copy `FintechDashboard.Api/Properties/launchSettings.Template.json` to `FintechDashboard.Api/Properties/launchSettings.json`.
2. Fill in your own API keys in the new file.

> Note: `launchSettings.json` is gitignored for security, so each developer must create their own local copy.

Example section:
```json
"environmentVariables": {
  "ASPNETCORE_ENVIRONMENT": "Development",
  "ALPHAVANTAGE_API_KEY": "your_alphavantage_key",
  "NEWSAPI_API_KEY": "your_newsapi_key",
  "OPENAI_API_KEY": "your_openai_key"
}
```

### Running with Environment Variables (PowerShell Example)

You can also set environment variables in your terminal before running the API:

```powershell
$env:ALPHAVANTAGE_API_KEY="your_alphavantage_key"
$env:NEWSAPI_API_KEY="your_newsapi_key"
$env:OPENAI_API_KEY="your_openai_key"
dotnet run --project FintechDashboard.Api
```

### Deployment (Azure, etc.)

Set these environment variables in your cloud provider's configuration panel (e.g., Azure App Service → Configuration → Application settings).

### If You Clone This Repo
- You must set your own API keys as environment variables before running the API.
- See the instructions above for how to do this. 
