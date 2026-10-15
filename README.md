# LockDownHostID Error Fix Tool

A Windows utility to resolve **Apple Device Trust (0xE) Errors** by removing corrupted pairing certificates in the Lockdown folder.

## 📋 Overview

When connecting an iPhone/iPad to Windows, you may encounter trust errors (error 0xE) preventing sync, backup, or development. This tool deletes the problematic `C:\ProgramData\Apple\Lockdown` folder, forcing Windows to generate fresh pairing certificates on next connection.

## ✨ Features

- **One-click fix** — Removes corrupted Lockdown certificates
- **Admin check** — Warns if not running as Administrator
- **Restart helper** — Optional PC restart after fix
- **Donate button** — Support the developer (GCash/PayMaya)
- **Contact link** — Facebook profile link
- **Modern UI** — Clean, responsive Windows Forms interface
- **Embedded icon** — Custom icon in titlebar, taskbar, and Explorer
- **Single-file EXE** — No installation, no .NET runtime required

## 🚀 Usage

1. **Download** `LockDownHostIDFixTool.exe`
2. **Right-click → Run as Administrator** (required)
3. Click **"✨ Apply Fix"**
4. Reconnect your Apple device or click **"🔄 Restart PC"**

## 📦 Distribution

The executable is a **true single-file, self-contained** application (~52 MB):
- No installer needed
- No .NET runtime required
- Runs on Windows 10/11 x64 out of the box

## 🛠️ Building from Source

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Windows 10/11 x64

### Build Commands
```bash
# Restore dependencies
dotnet restore

# Build debug
dotnet build

# Publish single-file release
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Output: `bin/Release/net10.0-windows/win-x64/publish/LockDownHostIDFixTool.exe`

## 📁 Project Structure

```
LockDownHostIDFixTool/
├── Form1.cs              # Main UI & logic
├── Form1.Designer.cs     # Designer partial class
├── Program.cs            # Entry point
├── LockDownHostIDFixTool.csproj
├── Apple Lockdown ID Fix Icon.ico
└── bin/Release/.../publish/LockDownHostIDFixTool.exe  # Distribution artifact
```

## ⚠️ Requirements

- **Windows 10/11 (x64)**
- **Administrator privileges** (mandatory for deleting system folder)
- Apple device drivers installed (iTunes or Apple Mobile Device Support)

## 🔧 Technical Details

- **Target Framework:** .NET 10.0 (Windows)
- **UI Framework:** Windows Forms
- **Publish Mode:** Single-file, self-contained, compressed
- **Trimming:** Disabled (WinForms compatibility)
- **Icon:** Embedded resource (works in single-file mode)

## 📞 Contact & Support

- **Developer:** Renz Cabangal (Alph1Tech)
- **Facebook:** [https://www.facebook.com/RenzCabangal18/](https://www.facebook.com/RenzCabangal18/)
- **Donate (GCash/PayMaya):** `09368005389`

## 📄 License

Free to use and distribute. Attribution appreciated.

---

**Note:** This tool modifies system folders. Use at your own risk. Always ensure you have a backup of important data.