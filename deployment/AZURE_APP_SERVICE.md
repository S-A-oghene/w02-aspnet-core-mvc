# Publish the ASP.NET Core MVC Application to a Public Website

This guide explains one practical route to obtain the public website URL requested by the assignment: **Azure App Service**. The app is configured to put its SQLite database under `/home/data` when the `MOVIES_DB_PATH` setting is supplied.

## Important notes before creating cloud resources

- Azure requires an account and subscription. Account eligibility, verification, regional capacity, and available offers can differ.
- The Azure App Service **F1 Free** plan is intended for trials, experimentation, and learning. Microsoft's current pricing page lists shared CPU, 60 CPU minutes per day, 1 GB storage, and no SLA; Microsoft does not support using F1 for production workloads. Review the current price and terms before creating resources: <https://azure.microsoft.com/en-us/pricing/details/app-service/linux/>.
- Do not upgrade to a paid tier unless you understand and accept the charge. If F1 is unavailable for your subscription or region, stop and ask your instructor which hosting option they accept rather than clicking a paid option by mistake.
- A public website URL is only created after the app has been deployed successfully. The URL cannot be generated from the ZIP alone.

## 1. Prepare the local app first

Before cloud deployment, the app must run locally:

```powershell
dotnet restore
dotnet build
dotnet run
```

Browse to `http://localhost:5080` and test the movie list. Stop the app with `Ctrl+C` before continuing.

## 2. Create or sign in to an Azure account

1. Open <https://portal.azure.com/>.
2. Sign in with your Microsoft account.
3. Complete any account/subscription verification requested by Azure.
4. Check that you can access an Azure subscription. If you cannot create an App Service, do not choose a paid plan as a workaround without first checking the price.

## 3. Install the Azure App Service extension in VS Code

1. Open Visual Studio Code.
2. Select Extensions (`Ctrl+Shift+X`).
3. Search for **Azure App Service** by Microsoft.
4. Install it.
5. Select the Azure icon in the VS Code activity bar.
6. Sign in to Azure when asked and choose your subscription.

Microsoft's VS Code quickstart describes using the Azure App Service extension and the command **Azure App Service: Create New Web App (Advanced)**. See <https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore>.

## 4. Create an App Service web application

1. Open the Command Palette with `Ctrl+Shift+P`.
2. Search for and select **Azure App Service: Create New Web App (Advanced)**.
3. If asked, sign in and select the subscription you intend to use.
4. Choose an existing resource group or create one named something such as `W02-Movie-Assignment-RG`.
5. Choose a globally unique web-app name. Use lowercase letters/numbers and a hyphen if needed. For example, `eseoghene-movies-w02-2026-xyz`. The example may already be taken, so choose a unique name.
6. When asked for the runtime stack, select **.NET 10 (LTS)**.
7. Select **Linux** as the operating system.
8. Choose an available region reasonably close to your likely visitors. Select from the regions Azure actually offers to your subscription.
9. Create a new App Service plan and select **Free (F1)** only if Azure shows it as available and the displayed cost is zero.
10. Review the summary and create the web app.
11. Wait for the resource-creation notification to report success.

Azure may assign a secure unique default hostname rather than exactly the name you typed. Copy the actual hostname shown in the App Service Overview page; do not guess it.

## 5. Configure persistent SQLite storage

The local project normally stores the database under `App_Data`. On Azure Linux App Service, set the database location to the persistent `/home` directory so the file does not sit inside the deployed application folder.

1. In VS Code, open the Azure panel.
2. Expand your subscription and **App Services**.
3. Expand/select the new web app. Right-click it and choose **Open in Portal** (or open the same resource in the Azure Portal).
4. In the web app, open **Settings → Environment variables** or **Configuration** (the exact label can vary in the portal).
5. Add an application setting:

   | Name | Value |
   |---|---|
   | `MOVIES_DB_PATH` | `/home/data/eseoghene-movies.db` |
   | `WEBSITES_ENABLE_APP_SERVICE_STORAGE` | `true` |

6. Save/apply the settings. Azure may restart the application after saving.

The application creates the `/home/data` directory if it does not exist and then creates/updates the SQLite file. Microsoft documents that App Service's `/home` storage can persist files across restarts when storage persistence is enabled. See <https://learn.microsoft.com/en-us/azure/app-service/configure-custom-container> and <https://learn.microsoft.com/en-us/azure/app-service/reference-app-settings>.

## 6. Deploy from Visual Studio Code

1. Return to VS Code.
2. Select **File → Open Folder...** and open the inner `EseogheneMovies` folder (the folder that contains `EseogheneMovies.csproj`).
3. Wait for C# Dev Kit to finish loading the project.
4. Open the Command Palette (`Ctrl+Shift+P`).
5. Search for **Azure App Service: Deploy to Web App** and select it.
6. Choose the `EseogheneMovies` folder as the folder to deploy.
7. Select the Azure subscription you used when creating the web app.
8. Select the web app you created in Step 4.
9. Confirm the deployment if VS Code asks.
10. Wait until VS Code reports that deployment has completed successfully. Do not close VS Code while the deployment is running.

If deployment fails, read the full VS Code Output panel and the Azure deployment notification. Confirm `dotnet build` succeeds locally and that the App Service runtime is set to .NET 10.

## 7. Open the live application

1. In the VS Code Azure panel, right-click your web app and select **Browse Website**, or open the App Service Overview page in the Azure Portal and copy **Default domain**.
2. The address should begin with `https://` and end in the Azure-provided `azurewebsites.net` domain.
3. Open it in Chrome or Edge.
4. Confirm that **Eseoghene Movies** appears and that **My Favorite Movies** is the page heading.
5. Test the search and CRUD features on the deployed website.
6. Copy the exact live URL from the browser address bar. This is the website URL for the assignment.

## 8. Personalise the live database

The cloud app has its own database; it is not automatically the same as your local App_Data database. On the live website:

1. Edit the three starter examples to your own favourite movies, or delete them and add your own.
2. Confirm that at least three genuinely favourite movies appear.
3. Test the year dropdown on the live site.
4. Restarting/redeploying should not erase the SQLite file when the persistent `/home` storage setting is working. Verify this by reopening the site after a restart.

## 9. If you change the source code later

1. Save the changed files.
2. Run `dotnet build` locally.
3. Test locally.
4. Run **Azure App Service: Deploy to Web App** again and choose the same app.
5. Refresh the website with `Ctrl+F5`.

## 10. Do not submit until both URLs work

- **Website URL:** the live HTTPS address provided by Azure.
- **Repository URL:** the GitHub repository address after you push the source code.

A public website URL and a GitHub repository URL are two different things. Submit both if Canvas provides two fields. Never submit the local-only `http://localhost:5080` address.

## Alternative: use GitHub Actions for repeatable deployments

For students who later want automatic deployments, Azure App Service can connect to a GitHub repository through its Deployment Center. Microsoft documents GitHub Actions deployment in the App Service quickstart: <https://learn.microsoft.com/en-us/azure/app-service/quickstart-dotnetcore>. This is optional for this first assignment; the VS Code deployment method above avoids introducing a GitHub Actions workflow before the student has a working first deployment.
