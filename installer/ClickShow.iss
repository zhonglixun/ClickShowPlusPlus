; ClickShow++ 安装包脚本（full = 自包含，lite = 需 .NET 8 桌面运行时）
#define MyAppName "ClickShow++"
#define MyAppVersion "2.0.0"
#define MyAppExeName "ClickShow.exe"

#ifndef BuildFlavor
  #define BuildFlavor "full"
#endif

#if BuildFlavor == "lite"
  #define PublishDir "..\publish-fd"
  #define SetupSuffix "-lite"
#else
  #define PublishDir "..\publish"
  #define SetupSuffix ""
#endif

[Setup]
AppId={{8D5F0C32-9A47-4E5B-B61A-3F2D5C8E9A10}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={autopf}\ClickShow
DefaultGroupName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
OutputDir=Output
OutputBaseFilename=ClickShow-Setup-{#MyAppVersion}{#SetupSuffix}
Compression=lzma2
SolidCompression=yes
ArchitecturesInstallIn64BitMode=x64compatible
PrivilegesRequired=admin
CloseApplications=yes
RestartApplications=no
WizardStyle=modern

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"
Name: "zhcn"; MessagesFile: "ChineseSimplified.isl"
Name: "ja"; MessagesFile: "compiler:Languages\Japanese.isl"
Name: "ko"; MessagesFile: "compiler:Languages\Korean.isl"
Name: "es"; MessagesFile: "compiler:Languages\Spanish.isl"
Name: "fr"; MessagesFile: "compiler:Languages\French.isl"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"
Name: "ptbr"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "ru"; MessagesFile: "compiler:Languages\Russian.isl"
Name: "it"; MessagesFile: "compiler:Languages\Italian.isl"

[CustomMessages]
en.taskDesktop=Create a &desktop shortcut
en.taskStartMenu=Create a &Start Menu item
en.runApp=&Launch ClickShow
en.liteNote=This is the Lite build: it does not bundle the .NET runtime, so it is only ~1 MB. It requires the .NET 8 Desktop Runtime (x64) to be installed.
en.runtimeRequired=This is the Lite build and needs the .NET 8 Desktop Runtime (x64), which was not found on this PC. Click Yes to open the download page, install the runtime, then run this installer again.
zhcn.taskDesktop=创建桌面快捷方式(&D)
zhcn.taskStartMenu=创建开始菜单项(&S)
zhcn.runApp=运行 ClickShow(&R)
zhcn.liteNote=这是精简版（Lite）：不打包 .NET 运行时，体积仅约 1MB。运行前必须先安装 .NET 8 桌面运行时（x64）。
zhcn.runtimeRequired=这是精简版，需要安装 .NET 8 桌面运行时（x64），但本机没有检测到。点「是」打开下载页面，安装完成后重新运行本安装程序。
ja.taskDesktop=デスクトップにショートカットを作成(&D)
ja.taskStartMenu=スタートメニューに項目を作成(&S)
ja.runApp=ClickShow を起動(&L)
ja.liteNote=これは Lite 版です。.NET ランタイムを同梱しないため約 1MB です。.NET 8 デスクトップ ランタイム（x64）が必要です。
ja.runtimeRequired=これは Lite 版で、.NET 8 デスクトップ ランタイム（x64）が必要ですが、見つかりません。「はい」でダウンロード ページを開き、インストール後に再実行してください。
ko.taskDesktop=바탕 화면 바로 가기 만들기(&D)
ko.taskStartMenu=시작 메뉴 항목 만들기(&S)
ko.runApp=ClickShow 실행(&R)
ko.liteNote=이것은 Lite 버전입니다. .NET 런타임을 포함하지 않아 약 1MB이며, .NET 8 데스크톱 런타임(x64)이 필요합니다.
ko.runtimeRequired=Lite 버전은 .NET 8 데스크톱 런타임(x64)이 필요하지만 찾을 수 없습니다. '예'를 눌러 다운로드 페이지를 열고 설치 후 다시 실행하세요.
es.taskDesktop=Crear acceso &directo en el escritorio
es.taskStartMenu=Crear elemento en el menú &Inicio
es.runApp=&Ejecutar ClickShow
es.liteNote=Esta es la versión Lite: no incluye el runtime de .NET y ocupa ~1 MB. Requiere .NET 8 Desktop Runtime (x64).
es.runtimeRequired=Esta versión Lite requiere .NET 8 Desktop Runtime (x64) y no se ha encontrado. Pulsa Sí para abrir la página de descarga, instálalo y vuelve a ejecutar.
fr.taskDesktop=Créer un raccourci sur le &bureau
fr.taskStartMenu=Créer un élément dans le menu &Démarrer
fr.runApp=&Lancer ClickShow
fr.liteNote=Ceci est la version Lite : sans le runtime .NET, environ 1 Mo. Nécessite .NET 8 Desktop Runtime (x64).
fr.runtimeRequired=Cette version Lite nécessite .NET 8 Desktop Runtime (x64), introuvable. Cliquez sur Oui pour ouvrir la page de téléchargement, puis relancez.
de.taskDesktop=&Desktopverknüpfung erstellen
de.taskStartMenu=Eintrag im &Startmenü erstellen
de.runApp=&ClickShow starten
de.liteNote=Dies ist die Lite-Version: ohne .NET-Laufzeit, nur ca. 1 MB. Benötigt die .NET 8 Desktop Runtime (x64).
de.runtimeRequired=Diese Lite-Version benötigt die .NET 8 Desktop Runtime (x64), die nicht gefunden wurde. „Ja“ öffnet die Downloadseite; danach das Setup erneut starten.
ptbr.taskDesktop=Criar atalho na &área de trabalho
ptbr.taskStartMenu=Criar item no menu &Iniciar
ptbr.runApp=&Executar o ClickShow
ptbr.liteNote=Esta é a versão Lite: sem o runtime .NET, cerca de 1 MB. Requer o .NET 8 Desktop Runtime (x64).
ptbr.runtimeRequired=A versão Lite requer o .NET 8 Desktop Runtime (x64), que não foi encontrado. Clique em Sim para abrir a página de download e execute o instalador novamente.
ru.taskDesktop=Создать ярлык на &рабочем столе
ru.taskStartMenu=Создать пункт в меню «&Пуск»
ru.runApp=&Запустить ClickShow
ru.liteNote=Это облегчённая версия: без среды .NET, около 1 МБ. Требуется .NET 8 Desktop Runtime (x64).
ru.runtimeRequired=Для облегчённой версии нужен .NET 8 Desktop Runtime (x64), но он не найден. Нажмите «Да», чтобы открыть страницу загрузки, затем запустите установку снова.
it.taskDesktop=Crea &collegamento sul desktop
it.taskStartMenu=Crea voce nel menu &Start
it.runApp=&Avvia ClickShow
it.liteNote=Questa è la versione Lite: senza runtime .NET, circa 1 MB. Richiede .NET 8 Desktop Runtime (x64).
it.runtimeRequired=La versione Lite richiede .NET 8 Desktop Runtime (x64), non trovato. Premi Sì per aprire la pagina di download, poi riesegui il programma di installazione.

[Tasks]
Name: "desktopicon"; Description: "{cm:taskDesktop}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "startmenuicon"; Description: "{cm:taskStartMenu}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "{#PublishDir}\*"; DestDir: "{app}"; Excludes: "*.pdb"; Flags: ignoreversion recursesubdirs createallsubdirs

#if BuildFlavor == "lite"
[InstallDelete]
Type: files; Name: "{app}\D3DCompiler_47_cor3.dll"
Type: files; Name: "{app}\PenImc_cor3.dll"
Type: files; Name: "{app}\PresentationNative_cor3.dll"
Type: files; Name: "{app}\vcruntime140_cor3.dll"
Type: files; Name: "{app}\wpfgfx_cor3.dll"
#endif

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: startmenuicon
Name: "{group}\卸载 {#MyAppName}"; Filename: "{uninstallexe}"; Tasks: startmenuicon
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:runApp}"; Flags: nowait postinstall skipifsilent

[Code]
// 根目录保护：用户选择磁盘根目录时自动套一层 ClickShow 文件夹
function NextButtonClick(CurPageID: Integer): Boolean;
var
  Dir: String;
begin
  Result := True;
  if CurPageID = wpSelectDir then
  begin
    Dir := WizardDirValue();
    if (Length(Dir) <= 3) and (Copy(Dir, 2, 1) = ':') then
    begin
      Dir := Dir + 'ClickShow';
      MsgBox('不能把文件直接散落在磁盘根目录，已自动改为安装到：' + #13#10 + Dir, mbInformation, MB_OK);
      WizardForm.DirEdit.Text := Dir;
    end;
  end;
end;

procedure InitializeWizard();
begin
#if BuildFlavor == "lite"
  WizardForm.WelcomeLabel2.Caption :=
    WizardForm.WelcomeLabel2.Caption + #13#10#13#10 + ExpandConstant('{cm:liteNote}');
#endif
end;

#if BuildFlavor == "lite"
function HasDotNet8DesktopRuntime(): Boolean;
var
  FindRec: TFindRec;
  BaseDir: String;
  Names: TArrayOfString;
  I: Integer;
begin
  Result := False;
  BaseDir := ExpandConstant('{commonpf64}\dotnet\shared\Microsoft.WindowsDesktop.App');
  if FindFirst(BaseDir + '\8.*', FindRec) then
  begin
    Result := True;
    FindClose(FindRec);
  end;
  if Result then
    Exit;
  if RegGetSubkeyNames(HKLM, 'SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall', Names) then
    for I := 0 to GetArrayLength(Names) - 1 do
      if Pos('Microsoft Windows Desktop Runtime - 8.', Names[I]) = 1 then
      begin
        Result := True;
        Exit;
      end;
  if RegGetSubkeyNames(HKLM, 'SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall', Names) then
    for I := 0 to GetArrayLength(Names) - 1 do
      if Pos('Microsoft Windows Desktop Runtime - 8.', Names[I]) = 1 then
      begin
        Result := True;
        Exit;
      end;
end;
#endif

// 安装前检测：lite 版检查 .NET 8 桌面运行时；两者都提示关闭正在运行的实例（配合 CloseApplications=yes）
function InitializeSetup(): Boolean;
var
  ResultCode: Integer;
begin
  Result := True;
#if BuildFlavor == "lite"
  if not HasDotNet8DesktopRuntime() then
  begin
    if MsgBox(ExpandConstant('{cm:runtimeRequired}'), mbError, MB_YESNO) = IDYES then
      ShellExec('open', 'https://dotnet.microsoft.com/download/dotnet/8.0', '', '', SW_SHOWNORMAL, ewNoWait, ResultCode);
    Result := False;
    Exit;
  end;
#endif
  Exec('tasklist', '/FI "IMAGENAME eq ClickShow.exe" /NH', '', 0, ewWaitUntilTerminated, ResultCode);
end;

