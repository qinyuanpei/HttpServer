FROM ubuntu:14.04
LABEL vendor="qinyuanpei@163.com"

# Install Mono && XBuild
RUN sudo apt-get update
RUN sudo apt-get upgrade -y
RUN sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
RUN sudo apt install apt-transport-https -y
RUN sudo apt-get install wget -y
RUN echo "deb https://download.mono-project.com/repo/ubuntu stable-trusty main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
RUN sudo apt-get update
RUN sudo apt-get install aptitude -y
RUN sudo apt-get install -f
RUN sudo apt-get install -y git
RUN sudo apt-get install -y zip
RUN sudo apt-get install -y unzip
RUN sudo aptitude install -y mono-complete

# Intall Nuget
RUN sudo wget -O nuget.exe https://dist.nuget.org/win-x86-commandline/v4.6.2/nuget.exe 
#RUN alias nuget="mono nuget.exe"

# Install Sonar-Scanner
RUN sudo wget -O sonar-scanner.zip https://github.com/SonarSource/sonar-scanner-msbuild/releases/download/4.3.0.1333/sonar-scanner-msbuild-4.3.0.1333-net46.zip
RUN sudo unzip sonar-scanner.zip
#RUN alias sonar-scanner="mono ./sonar-scanner/SonarQube.Scanner.MSBuild.exe"

# Install NUnit
RUN mono nuget.exe install NUnit.Runners -Version 3.8.0 -OutputDirectory ./TestRunner
#RUN alias nunit="mono ./TestRunner/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe"

# Build Project && Sonar Analyse && UnitTest
RUN git clone https://github.com/qinyuanpei/HttpServer.git
RUN mono ./sonar-scanner/SonarQube.Scanner.MSBuild.exe begin /k:"Sonar-HttpServer" /d:sonar.organization="qinyuanpei-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="db795a28468dc7c12805b330afed53d362fdd2d9"
RUN msbuild /p:Configuration=Release ./HttpServer/HTTPServer/HTTPServer.sln
RUN mono ./sonar-scanner/SonarQube.Scanner.MSBuild.exe end /d:sonar.login="db795a28468dc7c12805b330afed53d362fdd2d9"
RUN mono ./TestRunner/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe ./HttpServer/HTTPServer/HTTPServerLib.UnitTest/bin/Release/HttpServerLib.UnitTest.dll
EXPOSE 2048
