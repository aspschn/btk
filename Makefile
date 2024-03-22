dev:
	dotnet build -c Release -property:DefineConstants=BLUSHER_LIBFOUNDATION_DEV

build:
	dotnet build -c Release
