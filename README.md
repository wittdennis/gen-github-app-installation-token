# gen-github-app-installation-token

Simple tool to generate a Github App [installation access token](https://docs.github.com/en/apps/creating-github-apps/authenticating-with-a-github-app/generating-an-installation-access-token-for-a-github-app).

## Usage

```sh
docker run -e GITHUB_APP_CLIENT_ID=<YOUR-GITHUB-APP-CLIENT-ID> -e GITHUB_APP_INSTALLATION_ID=<YOUR-GITHUB-APP-INSTALLATION-ID> -e GITHUB_APP_PRIVATE_KEY=<YOUR-GITHUB-APP-PRIVATE-KEY> denniswitt/gen-github-app-installation-token:latest
```
