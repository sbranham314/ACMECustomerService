#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

ENV CONSUL_URL = http://127.0.0.1:3500/
ENV HOST = 127.0.0.1:3500
EXPOSE 80
EXPOSE 443
RUN sed -i 's/TLSv1.2/TLSv1.0/g' /etc/ssl/openssl.cnf

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ACMECorpCustomerService.csproj", "."]
RUN dotnet restore "./ACMECorpCustomerService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ACMECorpCustomerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ACMECorpCustomerService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/mssql/server

COPY setup.sql setup.sql
COPY setup_database.sh setup_database.sh
COPY entrypoint.sh entrypoint.sh

CMD /bin/bash ./entrypoint.sh

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ACMECorpCustomerService.dll"]