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
RUN sudo mkdir /usr/nuget/
RUN sudo wget -O /usr/nuget/nuget.exe https://dist.nuget.org/win-x86-commandline/v4.6.2/nuget.exe 
ENV NUGET="/usr/nuget/nuget.exe"

# Install Sonar-Scanner
RUN sudo wget -O sonar-scanner.zip https://sonarsource.bintray.com/Distribution/sonar-scanner-cli/sonar-scanner-cli-3.2.0.1227-linux.zip
RUN sudo unzip sonar-scanner.zip -d /usr/
RUN sudo chmod -R 777 usr/sonar-scanner-3.2.0.1227-linux
ENV SONAR_SCANNER="usr/sonar-scanner-3.2.0.1227-linux/bin/sonar-scanner"

# Install NUnit
RUN mono ${NUGET} install NUnit.Runners -Version 3.8.0 -OutputDirectory ./TestRunner
ENV NUNIT="./TestRunner/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe"

# Build Project && Sonar Analyse && UnitTest
RUN sudo mkdir ./WorkSpace/
RUN cd ./WorkSpace/
RUN sudo chmod -R 777 ./WorkSpace/
RUN git clone https://github.com/qinyuanpei/HttpServer.git 
RUN cd ./HttpServer/
RUN sudo ${SONAR_SCANNER} -D sonar.host.url="https://sonarcloud.io" -D sonar.login="db795a28468dc7c12805b330afed53d362fdd2d9" -D sonar.projectKey="Sonar-HttpServer" -D sonar.sources="." -D sonar.projectName="HttpServer" -X
RUN msbuild /p:Configuration=Release ./HTTPServer/HTTPServer.sln
RUN mono ${NUNIT} ./HTTPServer/HTTPServerLib.UnitTest/bin/Release/HttpServerLib.UnitTest.dll
EXPOSE 2048
