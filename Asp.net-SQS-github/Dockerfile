#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Asp.net-SQS-github/Asp.net-SQS-github.csproj", "Asp.net-SQS-github/"]
RUN dotnet restore "Asp.net-SQS-github/Asp.net-SQS-github.csproj"
COPY . .
WORKDIR "/src/Asp.net-SQS-github"
RUN dotnet build "Asp.net-SQS-github.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Asp.net-SQS-github.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Asp.net-SQS-github.dll"]