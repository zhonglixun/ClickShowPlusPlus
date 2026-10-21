[简体中文](README.md) | [English](README.en-US.md) | [日本語](README.ja-JP.md) | [한국어](README.ko-KR.md) | [Español](README.es-ES.md) | [Français](README.fr-FR.md) | [Deutsch](README.de-DE.md) | **Português (Brasil)** | [Русский](README.ru-RU.md) | [Italiano](README.it-IT.md)

# ClickShow 2.0

Embelezador de cliques do mouse para Windows. Cada clique esquerdo, do meio, direito, X1 e X2 expande uma onda colorida junto ao cursor.

![Demonstração](https://user-images.githubusercontent.com/1972649/122925974-f17ead00-d399-11eb-9c57-9b2f14dd5973.gif)

![Ajustes](docs/screenshot-settings.png)

> Reescrita moderna do [cuiliang/ClickShow](https://github.com/cuiliang/clickshow) (baseada na 1.4.1). Obrigado ao autor original. Interface e funções totalmente reescritas; apenas Windows 10/11.

## Funções

### Onda do clique
- Cor e liga/desliga independentes para os cinco botões (esquerdo/meio/direito/X1/X2), com seletor de cores e entrada hexadecimal;
- **8 temas de cores predefinidos**: Clássico / Neón noturno / Caramelo / Oceano / Pôr do sol / Floresta / Frutas vermelhas / Macaron — aplicação com um clique;

![Temas predefinidos](docs/theme-presets.png)

- **Duração da animação** ajustável de 200 a 600 ms;
- **5 estilos de difusão**: linear / desaceleração (natural) / frenagem forte (água) / leve rebote / elástico;
- Pré-visualização com fundo duplo: teste o efeito em cenários claros e escuros na hora.

### Perfis claro e escuro
Os temas claro e escuro são salvos separadamente (tamanho, cores, duração, estilo); seguem automaticamente o tema do sistema ou ficam fixos em um.

### Ponto do mouse
Indicador circular que segue o cursor, com tamanho e cor ajustáveis e ativação independente.

### Silêncio
- **Ocultar em tela cheia**: sem efeitos quando a janela em primeiro plano preenche a tela — jogos e vídeos limpos;
- **Silêncio por programa**: adicione programas (digite, navegue ou capture o primeiro plano) para ocultar o efeito enquanto estiverem ativos.

### Idioma da interface
简体中文 / English / 日本語 / 한국어 / Español / Français / Deutsch / Português (Brasil) / Русский / Italiano. Segue o idioma do sistema na primeira execução; alterável nas configurações.

### Bandeja
- Clique esquerdo no ícone da bandeja abre as configurações;
- Menu de contexto: abrir configurações / pausar onda / sair;
- A pausa surte efeito na hora e é restaurada sem alterar nada.

## Requisitos

Windows 10 1809+ ou Windows 11 (x64). Runtime incluído — **não precisa instalar o .NET**.

## Instalação

1. Baixe o ClickShow-Setup-2.0.0.exe na página de Releases deste repositório;
2. Execute o assistente: caminho de instalação personalizado (a raiz de uma unidade recebe uma pasta automaticamente); atalhos opcionais na área de trabalho e no menu iniciar;
3. Se houver uma versão antiga em execução, o instalador pedirá para encerrá-la.

Versão portátil: baixe o ClickShow-2.0.0-portable.zip, descompacte e execute — sem instalação; as configurações ficam em %APPDATA%/ClickShow.

## Atualizar da 1.x

- Suas configurações antigas (`Documentos/ClickShow.setting`) migram automaticamente na primeira execução para `%APPDATA%/ClickShow/setting.json` — cores e opções preservadas;
- Os perfis claro e escuro começam como cópias idênticas;
- Reinstalar por cima mantém todas as configurações e a entrada de inicialização.

## Compilar

```
dotnet publish ClickShow/ClickShow.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

Instalador: compile `installer/ClickShow.iss` com o [Inno Setup 6](https://jrsoftware.org/isinfo.php).

## Perguntas frequentes

- **Sem onda em tela cheia?** O "ocultar em tela cheia" está fazendo o trabalho dele — desative nas configurações;
- **"Pausar onda" na bandeja está cinza?** O gancho do mouse falhou ao instalar (raro) — reinicie o aplicativo;
- **Entrada de inicialização quebrada após desinstalar?** O desinstalador limpa a entrada do registro automaticamente.

## Histórico

### 2.0.0 (2026-10)
- Interface reescrita: configurações em janela única, temas claro/escuro seguindo o sistema, 10 idiomas de interface;
- Renderização da onda refeita: animação por transformação de escala (quadros cheios em telas de alta frequência), antialiasing vetorial, duração e estilo de difusão ajustáveis;
- 8 temas de cores predefinidos, perfis claro e escuro independentes;
- Ocultação em tela cheia e silêncio por programa;
- Seletor de cores + hexadecimal; pausa pela bandeja; importar/exportar configurações;
- .NET Framework 4.7.2 → .NET 8 (arquivo único autocontido), instalador Inno Setup.

### 1.4.1 e anteriores
Veja as [versões do projeto original](https://github.com/cuiliang/clickshow/releases).

## Licença
Mesma licença do projeto original (ver LICENSE). Obrigado a [cuiliang](https://github.com/cuiliang) pelo ClickShow original.
