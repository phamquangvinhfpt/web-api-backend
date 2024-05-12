# How to start project on Visual Studio Code

## Install chocolatey
First, open terminal or command line with Administrator. Then install all of these below:
```shell
@"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command "[System.Net.ServicePointManager]::SecurityProtocol = 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"
```

## Install make
```shell
choco install make
```
## Start the project
First go to the Core project by:
```shell
cd Core
```
Then type make build to build project:
```shell
make build
```
Run project:
```shell
make start
```

## Test api:
> Go Core folder and open Core.http to test all api.