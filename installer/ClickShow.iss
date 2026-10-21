; ClickShow 2.0.0 安装包脚本
#define MyAppName "ClickShow++"
#define MyAppVersion "2.0.0"
#define MyAppExeName "ClickShow.exe"

[Setup]
AppId={{8D5F0C32-9A47-4E5B-B61A-3F2D5C8E9A10}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
DefaultDirName={autopf}\ClickShow
DefaultGroupName={#MyAppName}
UninstallDisplayIcon={app}\{#MyAppExeName}
OutputDir=Output
OutputBaseFilename=ClickShow-Setup-{#MyAppVersion}
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
zhcn.taskDesktop=创建桌面快捷方式(&D)
zhcn.taskStartMenu=创建开始菜单项(&S)
zhcn.runApp=运行 ClickShow(&R)
ja.taskDesktop=デスクトップにショートカットを作成(&D)
ja.taskStartMenu=スタートメニューに項目を作成(&S)
ja.runApp=ClickShow を起動(&L)
ko.taskDesktop=바탕 화면 바로 가기 만들기(&D)
ko.taskStartMenu=시작 메뉴 항목 만들기(&S)
ko.runApp=ClickShow 실행(&R)
es.taskDesktop=Crear acceso &directo en el escritorio
es.taskStartMenu=Crear elemento en el menú &Inicio
es.runApp=&Ejecutar ClickShow
fr.taskDesktop=Créer un raccourci sur le &bureau
fr.taskStartMenu=Créer un élément dans le menu &Démarrer
fr.runApp=&Lancer ClickShow
de.taskDesktop=&Desktopverknüpfung erstellen
de.taskStartMenu=Eintrag im &Startmenü erstellen
de.runApp=&ClickShow starten
ptbr.taskDesktop=Criar atalho na &área de trabalho
ptbr.taskStartMenu=Criar item no menu &Iniciar
ptbr.runApp=&Executar o ClickShow
ru.taskDesktop=Создать ярлык на &рабочем столе
ru.taskStartMenu=Создать пункт в меню «&Пуск»
ru.runApp=&Запустить ClickShow
it.taskDesktop=Crea &collegamento sul desktop
it.taskStartMenu=Crea voce nel menu &Start
it.runApp=&Avvia ClickShow

[Tasks]
Name: "desktopicon"; Description: "{cm:taskDesktop}"; GroupDescription: "{cm:AdditionalIcons}"
Name: "startmenuicon"; Description: "{cm:taskStartMenu}"; GroupDescription: "{cm:AdditionalIcons}"

[Files]
Source: "..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

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

// 安装前检测正在运行的 ClickShow（配合 CloseApplications=yes 提示关闭）
function InitializeSetup(): Boolean;
var
  ResultCode: Integer;
begin
  Result := True;
  Exec('tasklist', '/FI "IMAGENAME eq ClickShow.exe" /NH', '', 0, ewWaitUntilTerminated, ResultCode);
end;
