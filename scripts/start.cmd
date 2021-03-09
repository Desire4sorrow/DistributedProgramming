#скрипт запуска
start /d ..\Valuator\ dotnet run --no-build --urls "http://localhost:5001" #запуск двух экземпляров приложения с помощью прямого указания директории запуска
start /d ..\Valuator\ dotnet run --no-build  --urls "http://localhost:5002"

start /d..\nginx\ nginx.exe #запуск ярлыка nginx из директории dev