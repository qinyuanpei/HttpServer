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
RUN sudo aptitude install -y mono-complete

# Intall Nuget
RUN sudo wget https://dist.nuget.org/win-x86-commandline/v4.6.2/nuget.exe

# Build Project
# RUN msbuild -help
# RUN monn nuget restore <Solution File>
# RUN msbuild /p:Configuration=Release <Solution File> or <MSBuild Script File>

# Sonar Analyse
# Prepare sonar-project.properties file && Sounar-Runner so that Sonar can submit analyse result to backend.
# RUN sonnar-scanner

# Unit Test
# RUN mono nuget install NUnit.Runners -Version 3.8.0 -OutputDirectory ./TestRunner
# RUN mono ./TestRunner/NUnit.ConsoleRunner.3.8.0/tools/nunit3-console.exe <UnitTest.dll>
RUN git clone https://github.com/qinyuanpei/HttpServer.git
RUN ls 
RUN cd ./HttpServer
RUN msbuild ./HTTPServer/HTTPServe.sln
EXPOSE 2048
