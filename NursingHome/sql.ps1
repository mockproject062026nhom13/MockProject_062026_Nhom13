# Đọc file .env
$envFile = ".env"

$env:SA_PASSWORD = (
    Get-Content $envFile |
    Where-Object { $_ -match '^MSSQL_SA_PASSWORD=' } |
    ForEach-Object { ($_ -split '=',2)[1] }
)

docker exec -it nursinghome-db `
    /opt/mssql-tools18/bin/sqlcmd `
    -S localhost `
    -U sa `
    -P $env:SA_PASSWORD `
    -d NursingHome `
    -C