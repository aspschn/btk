dev:
	dotnet build -c Release -property:DefineConstants=BTK_LIBSWINGBY_DEV

build:
	dotnet build -c Release
