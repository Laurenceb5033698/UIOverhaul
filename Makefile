all:
	dotnet build
	copy /y /v "C:\Users\herbglitch\source\repos\UIOverhaul\UIOverhaul\bin\Debug\netstandard2.0\UIOverhaul.dll" "D:\Program Files (x86)\Steam\steamapps\common\Risk of Rain 2\BepInEx\plugins\"

test:
	dotnet build
	copy /y /v "C:\Users\herbglitch\source\repos\UIOverhaul\UIOverhaul\bin\Debug\netstandard2.0\UIOverhaul.dll" "D:\Program Files (x86)\Steam\steamapps\common\Risk of Rain 2\BepInEx\plugins\"
	"D:\Program Files (x86)\Steam\steamapps\common\Risk of Rain 2\Risk of Rain 2.exe"