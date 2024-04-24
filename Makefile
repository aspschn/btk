dev:
	dotnet build -c Release -property:DefineConstants=BLUSHER_LIBSWINGBY_DEV

build:
	dotnet build -c Release
