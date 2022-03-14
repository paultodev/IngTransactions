#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["IngTransactions/IngTransactions.csproj", "IngTransactions/"]
COPY ["DAL/DAL.csproj", "DAL/"]
RUN dotnet restore "IngTransactions/IngTransactions.csproj"
COPY . .
WORKDIR "/src/IngTransactions"
RUN dotnet build "IngTransactions.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IngTransactions.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IngTransactions.dll"]