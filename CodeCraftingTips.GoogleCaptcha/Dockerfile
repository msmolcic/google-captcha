FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["CodeCraftingTips.GoogleCaptcha/CodeCraftingTips.GoogleCaptcha.csproj", "CodeCraftingTips.GoogleCaptcha/"]
RUN dotnet restore "CodeCraftingTips.GoogleCaptcha/CodeCraftingTips.GoogleCaptcha.csproj"
COPY . .
WORKDIR "/src/CodeCraftingTips.GoogleCaptcha"
RUN dotnet build "CodeCraftingTips.GoogleCaptcha.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeCraftingTips.GoogleCaptcha.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CodeCraftingTips.GoogleCaptcha.dll"]