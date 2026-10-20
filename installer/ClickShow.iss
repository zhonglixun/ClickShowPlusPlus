; ClickShow 2.0.0 安装包脚本
#define MyAppName "ClickShow"
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
Name: "chinesesimplified"; MessagesFile: "ChineseSimplified.isl"

[Tasks]
Name: "desktopicon"; Description: "创建桌面快捷方式(&D)"; GroupDescription: "附加任务:"
Name: "startmenuicon"; Description: "创建开始菜单项(&S)"; GroupDescription: "附加任务:"

[Files]
Source: "..\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: startmenuicon
Name: "{group}\卸载 {#MyAppName}"; Filename: "{uninstallexe}"; Tasks: startmenuicon
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "运行 {#MyAppName}"; Flags: nowait postinstall skipifsilent

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
