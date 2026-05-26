# 1. Aşama: Uygulamayı Derleme
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /App

# Proje dosyasını kopyala ve bağımlılıkları indir
COPY phishing.csproj ./
RUN dotnet restore

# Kalan tüm dosyaları kopyala ve projeyi yayına hazırla (publish)
COPY . ./
RUN dotnet publish -c Release -o out

# 2. Aşama: Uygulamayı Çalıştırma
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App
COPY --from=build-env /App/out .

# Render'ın varsayılan port ayarlarına uyum sağla
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "phishing.dll"]
