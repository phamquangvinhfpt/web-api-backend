# How to start project on Visual Studio Code
 
## 🙌 Contributors Gallery

<!-- readme: contributors -start -->
<table>
	<tbody>
		<tr>
            <td align="center">
                <a href="https://github.com/ToanDC28">
                    <img src="https://avatars.githubusercontent.com/u/134987653?v=4" width="90;" alt="ToanDC28"/>
                    <br />
                    <sub><b>Đoàn Công Toàn</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/DuongTranDangKhoa">
                    <img src="https://avatars.githubusercontent.com/u/126379959?v=4" width="90;" alt="DuongTranDangKhoa"/>
                    <br />
                    <sub><b>Khoa Dang</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/phamquangvinhfpt">
                    <img src="https://avatars.githubusercontent.com/u/76089021?v=4" width="90;" alt="phamquangvinhfpt"/>
                    <br />
                    <sub><b>Phạm Quang Vinh</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/HiepNguyenn2002">
                    <img src="https://avatars.githubusercontent.com/u/161460840?v=4" width="90;" alt="HiepNguyenn2002"/>
                    <br />
                    <sub><b>HiepNguyenn2002</b></sub>
                </a>
            </td>
            <td align="center">
                <a href="https://github.com/hoangthanhtuanfpt">
                    <img src="https://avatars.githubusercontent.com/u/126632292?v=4" width="90;" alt="hoangthanhtuanfpt"/>
                    <br />
                    <sub><b>Hoàng Thanh Tuấn</b></sub>
                </a>
            </td>
		</tr>
	<tbody>
</table>
<!-- readme: contributors -end -->

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
