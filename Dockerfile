FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS builder
WORKDIR /app

COPY . .
RUN dotnet publish RoboHash/RoboHash.csproj -c Release -o /app/out/ -r linux-x64 --self-contained


FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT "Development"

EXPOSE 80

COPY --from=builder /app/out/ .

ENTRYPOINT [ "./RoboHash" ]