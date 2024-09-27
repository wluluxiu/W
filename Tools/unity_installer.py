import base64
import io
import threading
import datetime
import os
import ctypes
import shutil
import time
import subprocess
import sys
import zipfile
import winreg
import platform
import threading

try:
    import win32gui
except ImportError:
    subprocess.call(['pip', 'install', 'pywin32'])

try:
    import requests
except ImportError:
    subprocess.call(['pip', 'install', 'requests'])
    import requests

try:
    from tqdm import tqdm
except ImportError:
    subprocess.call(['pip', 'install', 'tqdm'])
    from tqdm import tqdm

try:
    import tarfile
except ImportError:
    subprocess.call(['pip', 'install', 'tarfile'])
    import tarfile

try:
    import tkinter
except ImportError:
    subprocess.call(['pip', 'install', 'tkinter'])
    import tkinter

try:
    import PIL
except ImportError:
    subprocess.call(['pip', 'install', 'pillow'])
    import PIL

try:
    import pyzipper
except ImportError:
    subprocess.call(['pip', 'install', 'pyzipper'])
    import pyzipper

try:
    import psutil
except ImportError:
    subprocess.call(['pip', 'install', 'psutil'])
    import psutil

try:
    import cryptography
except ImportError:
    subprocess.call(['pip', 'install', 'cryptography'])
    import cryptography

from cryptography.fernet import Fernet

import tkinter as tk
from tkinter import ttk
from tkinter import messagebox
from tkinter import scrolledtext
from PIL import Image, ImageTk

ico64 = (
    "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"
)

unity_version = "2022.3.32f1"
os.chdir(os.path.dirname(os.path.abspath(__file__)))
dir = os.getcwd()
temp_path = os.path.join(dir, "Temp")

crack_key = "zDxBtEHrdTZDUPKwZ7HVlQlBQEyW8R3ilwXxxD31l6o="
crack_pwd = "gAAAAABlElEFP2fI0MoCnUUsBFkl8IcUVzqW0zSaa37mChQyX5TBdU3jnMOwHtk_OutTJ7cDU59paX7hggreldjYvpFvdbE-qA=="

# 下载到本地的路径
unity_hub_setup_path = f"{temp_path}/UnityHubSetup.exe"
unity_setup_path = f"{temp_path}/UnitySetup64.exe"
unity_android_setup_path = f"{temp_path}/UnitySetup-Android-Support-for-Editor.exe"
unity_ios_setup_path = f"{temp_path}/UnitySetup-iOS-Support-for-Editor.exe"
unity_window_setup_path = f"{temp_path}/UnitySetup-Windows-IL2CPP-Support-for-Editor.exe"
unity_webgl_set_up = f"{temp_path}/UnitySetup-WebGL-Support-for-Editor.exe"
unity_dk_zip_path = f"{temp_path}/DK.zip"
license_generate_path = f"{temp_path}/Crack/LicenseGenerate.exe"

# 安装相关
unity_hub_install_path = f"C:/Program Files/Unity Hub"
unity_install_path = f"C:/Program Files/Unity/Hub/Editor/{unity_version}"
unity_editor_path = f"{unity_install_path}/Editor"
unity_dk_install_path = f"{unity_editor_path}/Data/PlaybackEngines/AndroidPlayer"
unity_license_path = f"C:/ProgramData/Unity/Unity_lic.ulf"

#Unity Hub 公司内网下载地址  
download_win_url = [
    "http://192.168.9.95:8081/repository/jjpdcrelease/unity/hacker/windows/20220332f1/windows-20220332f1.tar.gz",
]

download_mac_url = [
    "http://192.168.9.95:8081/repository/jjpdcrelease/unity/hacker/windows/20220332f1/windows-20220332f1.tar.gz",
]

def stop_process(winName):
    for proc in psutil.process_iter(["name"]):
        if proc.info["name"] == winName:
            proc.kill()


def enable_long_paths():
    key_path = "SYSTEM\\CurrentControlSet\\Control\\FileSystem"
    with winreg.OpenKey(
        winreg.HKEY_LOCAL_MACHINE, key_path, 0, winreg.KEY_READ
    ) as reg_key:
        value, type = winreg.QueryValueEx(reg_key, "LongPathsEnabled")
        if value == 1:
            return
    with winreg.OpenKey(
        winreg.HKEY_LOCAL_MACHINE, key_path, 0, winreg.KEY_SET_VALUE
    ) as reg_key:
        winreg.SetValueEx(reg_key, "LongPathsEnabled", 0, winreg.REG_DWORD, 1)
    messagebox.showinfo("提示", "设置长路径成功！点击确认重启安装程序！", parent=root)
    os.startfile(sys.executable)
    sys.exit(0)

def download_file(url, save_path, progress):
    log(f"开始下载：{os.path.basename(url)}")
    print(f"开始下载：{os.path.basename(url)}, 保存路径：{save_path}")
    if not os.path.exists(os.path.dirname(save_path)):
        os.makedirs(os.path.dirname(save_path))
    response = requests.get(url, stream=True)
    total_size = int(response.headers.get("Content-Length", 0))
    # 已经下载过了
    if os.path.exists(save_path) and os.path.getsize(save_path) == total_size:
        progress(total_size, total_size, 0)
        return
    block_size = 1024  # 1 Kibibyte
    downloaded_size = 0
    speed = 0
    progress(downloaded_size, total_size, 0)
    start = datetime.datetime.now()
    with open(save_path, "wb") as f:
        for data in response.iter_content(block_size):
            f.write(data)
            current = datetime.datetime.now()
            downloaded_size += len(data)
            speed += len(data)
            delta = current - start
            if delta.total_seconds() > 0.5:
                progress(downloaded_size, total_size, speed / delta.total_seconds())
                start = current
                speed = 0
    progress(total_size, total_size, 0)

def extract_tar_gz_with_progress(archive_path, extraction_path):
    if os.path.exists(extraction_path):
        total_files = sum(1 for _ in tarfile.open(archive_path)) 
        with tarfile.open(archive_path, mode='r:gz') as tar:
            current_file = 0
            for member in tar:
                tar.extract(member, path=extraction_path)  # 解压文件
                current_file += 1
                progress = (current_file / total_files) * 100  # 计算进度百分比
                set_p2(f"解压进度: {progress:.2f}%", progress)

def extract_file_zip(save_path, extraPath):
    if os.path.exists(save_path):
        zip_ref = zipfile.ZipFile(save_path, "r")
        zip_ref.extractall(extraPath)

def extract_file_zip2_pwd(save_path, extraPath, pwd=None):
      if os.path.exists(save_path):
        with pyzipper.AESZipFile(save_path, 'r', compression=pyzipper.ZIP_DEFLATED, encryption=pyzipper.WZ_AES) as extracted_zip:
            if pwd != None:
                extracted_zip.pwd = pwd
            extracted_zip.extractall(extraPath)

def generate_license_file():
    import win32gui
    import win32con

    unity = f"{unity_editor_path}/Unity.exe"
    create_alf_command = f'"{unity}" -batchmode -createManualActivationFile'
    print(create_alf_command)
    subprocess.Popen(
        create_alf_command, creationflags=subprocess.CREATE_NO_WINDOW
    ).wait()
    alf_file = os.path.abspath(os.path.join(os.getcwd(), f"Unity_v{unity_version}.alf"))
    log(f"{alf_file}")
    while not os.path.exists(alf_file):
        time.sleep(1)

    # print('请使用 LicenseGenerate.exe 生成 Unity 许可证文件。'
    #       '\n具体步骤：'
    #       '\n1.点击“浏览ALF文件”'
    #       f'\n2.选择{alf_file}该文件'
    #       '\n3.点击创建Unity许可证'
    #       '\n4.关闭LicenseGenerate.exe')

    def wait_do(name, func, timeout=5, interval=0.1):
        start_time = time.time()
        while True:
            print(f"等待 {name} 完成...")
            result = func()
            time.sleep(interval)
            if result:
                return result
            if time.time() - start_time > timeout:
                raise Exception(f"等待超时: {name}")

    def dlg_set_path(dlg, path):
        def callback(hwnd, _path):
            cls = win32gui.GetClassName(hwnd)
            if cls == "Edit":
                _path = _path.replace("/", "\\")
                win32gui.SendMessage(hwnd, win32con.WM_SETTEXT, 0, _path)
            return True

        win32gui.EnumChildWindows(dlg, lambda hwnd, lParam: callback(hwnd, path), 0)

    def dlg_open(dlg):
        def callback(hwnd, lParam):
            txt = win32gui.GetWindowText(hwnd)
            if txt.find("打开") != -1 or txt.find("Open") != -1:
                win32gui.SendMessage(hwnd, win32con.BM_CLICK, 0, 0)
            return True

        win32gui.EnumChildWindows(dlg, callback, 0)

    stop_process("LicenseGenerate.exe")
    subprocess.Popen(license_generate_path)
    form = wait_do(
        "获取窗口句柄",
        lambda: win32gui.FindWindow(None, "Unity许可证生成工具 by：YXJ"),
    )
    brow = wait_do(
        "获取浏览文件按钮",
        lambda: win32gui.FindWindowEx(form, None, None, "浏览ALF文件"),
    )
    threading.Thread(
        target=lambda: win32gui.SendMessage(brow, win32con.BM_CLICK, 0, 0)
    ).start()
    dlg = wait_do(
        "获取文件选择对话框",
        lambda: win32gui.FindWindow(None, "请选择UnityHub生成的alf文件"),
    )
    dlg_set_path(dlg, alf_file)
    dlg_open(dlg)
    create = wait_do(
        "获取创建按钮",
        lambda: win32gui.FindWindowEx(form, None, None, "创建Unity许可证"),
    )
    time.sleep(0.5)
    threading.Thread(
        target=lambda: win32gui.SendMessage(create, win32con.BM_CLICK, 0, 0)
    ).start()
    tips = wait_do(
        "弹窗确认",
        lambda: win32gui.FindWindow(None, "提示"),
    )
    confirm = wait_do(
        "确定按钮",
        lambda: win32gui.FindWindowEx(tips, None, None, "确定"),
    )
    win32gui.SendMessage(confirm, win32con.BM_CLICK, 0, 0)
    win32gui.CloseWindow(form)
    stop_process("LicenseGenerate.exe")

    if os.path.exists(unity_license_path):
        print("正在安装许可证书...")
        manual_ulf_command = f'"{unity_editor_path}/Unity.exe" -batchmode -manualLicenseFile "{unity_license_path}"'
        subprocess.run(manual_ulf_command, shell=True)
    else:
        raise Exception("创建 Unity_lic.ulf 失败！")

    os.remove(alf_file)


def call_install(exe, install_dir="", silent=True):
    exe = os.path.abspath(exe)
    cmd = f'"{exe}"'
    if silent:
        cmd = f"{cmd} /S"
    if install_dir != "":
        win_dir = install_dir.replace("/", "\\")
        cmd = f"{cmd} /D={win_dir}"
    print(cmd)
    try:
        subprocess.Popen(cmd, creationflags=subprocess.CREATE_NO_WINDOW).wait()
    except Exception as e:
        raise Exception(f"安装失败！{exe}, {e}")


def step_check():
    if is_debug:
        return
    if not ctypes.windll.shell32.IsUserAnAdmin():
        raise Exception("请以管理员身份运行此程序！")
    try:
        subprocess.check_output(
            ["ping", "-n", "1", "unity.cn"], creationflags=subprocess.CREATE_NO_WINDOW
        )
        raise Exception("当前网络环境不支持安装！")
    except:
        return


def step_cleanup():
    stop_process("adb.exe")
    if os.path.exists(unity_dk_install_path):
        shutil.rmtree(unity_dk_install_path)

def step_download():
    if platform.system() == "Windows":
        download_url = download_win_url
    elif platform.system() == "Darwin":
        download_url = download_mac_url

    def download_callback(url, i, n, current, total, speed):
        down = f"下载安装包[{i+1}/{n}]: {os.path.basename(url)}, 速度：{speed/1024/1024:.1f}MB/s"
        set_p2(down, int(current / total * 100))

    for i, url in enumerate(download_url):
        save_path = os.path.join(dir, "hacker-20220332f1.tar.gz")
        download_file(
            url,
            save_path,
            lambda x, y, z: download_callback(url, i, len(download_url), x, y, z),
        )

def step_extract():
    if not os.path.exists(temp_path):
        os.makedirs(temp_path)
    save_path = os.path.join(dir, "hacker-20220332f1.tar.gz")
    extract_tar_gz_with_progress(save_path, temp_path)
    crack_zip_file_path = os.path.join(temp_path, "Crack.zip")
    fernet = Fernet(crack_key)
    plaintext = fernet.decrypt(crack_pwd)
    extract_file_zip2_pwd(crack_zip_file_path, temp_path, plaintext)

def step_install():
    set_p2("结束进程 Unity.exe, Unity Hub.exe", 10)
    stop_process("Unity.exe")
    stop_process("Unity Hub.exe")

    set_p2("安装 UnityHub", 15)
    log(f"开始安装 UnityHub")
    call_install(unity_hub_setup_path, f'"{unity_hub_install_path}"')

    set_p2(f"安装 Unity{unity_version}（此过程可能需要几分钟时间）", 20)
    log(f"开始安装 Unity{unity_version}（此过程可能需要几分钟时间）")
    call_install(unity_setup_path, unity_install_path)

    set_p2(f"安装 Unity{unity_version} Android Support", 40)
    log(f"开始安装 Unity{unity_version} Android Support")
    call_install(unity_android_setup_path)

    set_p2(f"安装 Unity{unity_version} iOS Support", 50)
    log(f"开始安装 Unity{unity_version} iOS Support")
    call_install(unity_ios_setup_path)

    set_p2(f"安装 Unity{unity_version} Windows IL2CPP Support", 60)
    log(f"开始安装 Unity{unity_version} Windows IL2CPP Support")
    call_install(unity_window_setup_path)

    set_p2(f"安装 Unity{unity_version} WebGL Support", 70)
    log(f"开始安装 Unity{unity_version} WebGL Support")
    call_install(unity_webgl_set_up)

    set_p2(f"安装 Unity{unity_version} Android SDK, NDK, JDK", 90)
    log(f"开始安装 Unity{unity_version} Android SDK, NDK, JDK")
    extract_file_zip(unity_dk_zip_path, unity_dk_install_path)

    # set_p2(f"替换 baseProjectTemplate.gradle ", 95)
    # log(f"替换 baseProjectTemplate.gradle")
    # target_gradle = os.path.join(f"{unity_dk_install_path}/Tools/GradleTemplates", "baseProjectTemplate.gradle")
    # crack_unity_file_gradle = os.path.join(temp_path, r"baseProjectTemplate.gradle")
    # shutil.copy2(crack_unity_file_gradle, target_gradle)

    # set_p2(f"替换 sdkmanager.bat ", 99)
    # log(f"替换 sdkmanager.bat")
    # target_bat = os.path.join(f"{unity_dk_install_path}/SDK/tools/bin", "sdkmanager.bat")
    # crack_unity_file_bat = os.path.join(temp_path, r"sdkmanager.bat")
    # shutil.copy2(crack_unity_file_bat, target_bat)


def step_crack():
    set_p2("替换文件", 0)
    # hub
    source_hub_asar = f"{temp_path}/Crack/HubCrack/app.asar"
    source_hub_dll = f"{temp_path}/Crack/HubCrack/System.Security.Cryptography.Xml.dll"
    target_hub_asar = f"{unity_hub_install_path}/resources/app.asar"
    target_hub_dll = f"{unity_hub_install_path}/UnityLicensingClient_V1/System.Security.Cryptography.Xml.dll"
    shutil.copy2(source_hub_asar, target_hub_asar)
    shutil.copy2(source_hub_dll, target_hub_dll)
    # unity
    source_unity_dll = (
        f"{temp_path}/Crack/UnityCrack/System.Security.Cryptography.Xml.dll"
    )
    target_unity_dll = f"{unity_editor_path}/Data/Resources/Licensing/Client/System.Security.Cryptography.Xml.dll"
    shutil.copy2(source_unity_dll, target_unity_dll)
    # license
    set_p2("生成证书", 30)
    generate_license_file()


def start_steps():
    steps = {
        "检查环境": step_check,
        "清理旧文件": step_cleanup,
        "下载安装包": step_download,
        "解压安装包": step_extract,
        "安装程序": step_install,
        "破解程序": step_crack,
    }
    success = True
    n = len(steps)
    for i, (name, step) in enumerate(steps.items()):
        log(f"步骤{i+1}: {name}, 开始执行...")
        set_p1(f"总进度[{i+1}/{n}]: {name}", 100.0 * i / n)
        set_p2(f"执行{name}", 0)
        time.sleep(0.5)
        try:
            step()
        except Exception as e:
            success = False
            log(f"步骤{i+1}: {name}, 执行失败: {e}")
            set_p1(f"总进度[{i+1}/{n}]: {name}, 执行失败", 100.0 * i / n)
            error_message = str(e)
            if "No module named 'win32gui'" in error_message:
                messagebox.showerror("错误", "关闭窗口，重新执行本程序", parent=root)
            else:
                messagebox.showerror("错误", f"{name}, 执行失败: {e}", parent=root)
            break
        log(f"步骤{i+1}: {name}, 执行完毕")
        time.sleep(0.5)
    if success:
        set_p1(f"安装完成", 100.0)
        set_p2(f"安装完成", 100.0)
        messagebox.showinfo("成功", f"安装完成！", parent=root)
    return success

root: tk.Tk = None
p1t: tk.Label = None
p1: ttk.Progressbar = None
p2t: tk.Label = None
p2: ttk.Progressbar = None
log_output: scrolledtext.ScrolledText = None
button: tk.Button = None
is_debug = sys.executable.lower().endswith("python.exe")
is_opened = False

def on_window_open(event):
    global is_opened
    if is_opened or is_debug:
        return
    is_opened = True
    try:
        enable_long_paths()
    except Exception as e:
        messagebox.showerror("错误", f"升级失败: {e}", parent=root)

def set_p1(text, value):
    global p1t, p1
    p1t.config(text=text)
    p1.config(value=value)


def set_p2(text, value):
    global p2t, p2
    p2t.config(text=text)
    p2.config(value=value)

def on_click_install():
    global button

    button.config(state=tk.DISABLED)
    button.config(text="正在安装...")

    def thread_install():
        start_steps()
        button.config(state=tk.NORMAL)
        button.config(text="一键安装")

    threading.Thread(target=thread_install).start()

def log(text):
    global log_output
    log_output.config(state=tk.NORMAL)
    log_output.insert(tk.END, f"{text}\n")
    log_output.config(state=tk.DISABLED)
    log_output.see(tk.END)

def tk_main():

    global root, p1t, p1, p2t, p2, button, log_output

    root = tk.Tk()
    root.geometry("600x400")
    root.title(f"Unity 安装工具 {dir}")

    # 设置图标
    image = ImageTk.PhotoImage(Image.open(io.BytesIO(base64.b64decode(ico64))))
    root.iconphoto(True, image)

    # 创建标签
    label = tk.Label(root, text=f"版本: Unity {unity_version}")
    label.pack()
    label.place(x=10, y=10)

    # 创建进度条 1
    p1t = tk.Label(root, text="步骤")
    p1t.pack()
    p1t.place(x=10, y=40)

    p1 = ttk.Progressbar(root, orient="horizontal", length=580)
    p1.pack()
    p1.place(x=10, y=60)

    # 创建进度条 2
    p2t = tk.Label(root, text="进度")
    p2t.pack()
    p2t.place(x=10, y=85)

    p2 = ttk.Progressbar(root, orient="horizontal", length=580)
    p2.pack()
    p2.place(x=10, y=105)

    # 创建一个带有滚动条的 Text 控件
    log_output = scrolledtext.ScrolledText(root, width=80, height=15)
    log_output.pack()
    log_output.place(x=10, y=150)

    # 创建按钮
    button = tk.Button(root, text="一键安装", command=on_click_install)
    button.pack(side="bottom", fill="x", pady=10, padx=10)

    # 绑定窗口打开事件
    root.bind("<Map>", on_window_open)

    # 禁止修改窗口
    root.resizable(False, False)

    # 启动
    root.mainloop()


tk_main()

# pyinstaller --onefile --uac-admin --noconsole --icon=app.ico unity_installer.py
