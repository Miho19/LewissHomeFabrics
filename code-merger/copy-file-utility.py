from pathlib import Path
import shutil
import os
from enum import Enum

import tkinter as tk

verbose: bool = True


class LoggingLevel(Enum):
    ALL = 1
    Standard = 2
    
loggingLevel = LoggingLevel.Standard


class TransferMode(Enum):
    COPY = 1
    MOVE = 2
    
def get_path_object(file_path:str) -> Path:
    try:
        if not os.access(file_path, os.F_OK):
            raise FileNotFoundError
        if not os.access(file_path, os.R_OK | os.W_OK):
            raise PermissionError
        return Path(file_path)
    except Exception as e:
        print(f"{type(e).__name__} {e=}\n{file_path}") 
        exit(1)
        
def create_dev_directory(directory_path: str)-> Path:
    try:
        dev_directory_path_object: Path = Path(directory_path)
          
        if not os.access(dev_directory_path_object, os.F_OK):
            dev_directory_path_object.mkdir(parents=False)
            if verbose: print(f"Created directory\t{dev_directory_path_object}")
        else:
            if verbose: print(f"Clearing directory\t{dev_directory_path_object}")
            clear_directory_content(dev_directory_path_object)
        
        return dev_directory_path_object
        
    except Exception as e:
        print(f"{type(e).__name__} {e=}\n{dev_directory_path_object}") 
        exit(1)
        
    

def clear_directory_content(directory_path: Path):
    try:
        
        directory_path_object: Path = get_path_object(directory_path)
        
        if not directory_path_object.exists():
            raise FileNotFoundError
        if not directory_path_object.is_dir():
            raise NotADirectoryError
        directory_content = directory_path_object.iterdir()
        
        for item in directory_content:
            if not item.is_file():
                continue
            item.unlink()
            if verbose and loggingLevel == LoggingLevel.ALL: print(f"Removed file\t{item.name}")
        
        
    except Exception as e:
        print(f"{type(e).__name__} {e=}") 
        exit(1)
        
def transfer_directory_files(source_directory_path: str, destination_directory_path: str, mode: TransferMode):
    try:
        
        source_directory_path_object: Path = get_path_object(source_directory_path)
        destination_directory_path_object: Path = get_path_object(destination_directory_path)
        
        if not source_directory_path_object.exists():
            raise FileNotFoundError
        if not source_directory_path_object.is_dir():
            raise NotADirectoryError
    
        source_directory_files = source_directory_path_object.iterdir()
        operation_name: str = "Copied" if mode == TransferMode.COPY else "Moved"
        
        for item in source_directory_files:
            if mode == TransferMode.MOVE:
                shutil.move(item, destination_directory_path_object)
            else:
                shutil.copy2(item, destination_directory_path_object)
            if verbose and loggingLevel == LoggingLevel.ALL: print(f"{operation_name} file {item.name:<15}\t{(destination_directory_path_object/item)}")
            
    except Exception as e:
        print(f"{type(e).__name__} {e=}") 
        exit(1)
        

def copy_file(source_path: str, destination_path: str):
    try:
        source_path_object: Path = get_path_object(source_path)
        if not source_path_object.exists():
            raise FileNotFoundError
        
        destination_path_object: Path = get_path_object(destination_path)
        shutil.copy2(source_path_object, destination_path_object)
        
    except Exception as e:
        print(f"{type(e).__name__} {e=}") 
        exit(1)
        
def push_code():
    
    source_directory_path: str = "C:/Users/japril/AppData/Roaming/LibreOffice/4/user/basic/Kinetics/"
    source_directory_path_object: Path = get_path_object(source_directory_path)
    
    destination_directory: str = "K:/Data/Process/Development/Macros/Kinetics"
    clear_directory_content(destination_directory)
    
    transfer_directory_files(source_directory_path, 
                             destination_directory, 
                             TransferMode.COPY)
    
    # Update metadata to point to files
    
    copy_file("//lhfwad01/Users/japril/Desktop/CopyFileUtility/User/script_prod.xlc",
              "C:/Users/japril/AppData/Roaming/LibreOffice/4/user/basic/script.xlc")
    
    copy_file("//lhfwad01/Users/japril/Desktop/CopyFileUtility/User/dialog_prod.xlc",
              "C:/Users/japril/AppData/Roaming/LibreOffice/4/user/basic/dialog.xlc")
    
    copy_file("//lhfwad01/Users/japril/Desktop/CopyFileUtility/Shared/script_prod.xlc",
              "C:/Program Files (x86)/LibreOffice/share/basic/script.xlc")
    
    copy_file("//lhfwad01/Users/japril/Desktop/CopyFileUtility/Shared/dialog_prod.xlc",
              "C:/Program Files (x86)/LibreOffice/share/basic/dialog.xlc")
    

def pull_code():
    
    # Get macro code from server
    source_macro_path: str = "K:/Data/Process/Development/Macros/Kinetics"

    destination_dir_path: str = "C:/Users/japril/AppData/Roaming/LibreOffice/4/user/basic/Kinetics"
    
    transfer_directory_files(source_macro_path, 
                             create_dev_directory(destination_dir_path), 
                             TransferMode.COPY)
    
    # Update metadata to point to files
    
    copy_file("//lhfwad01/Users/japril/Desktop/CopyFileUtility/User/script_dev.xlc",
              "C:/Users/japril/AppData/Roaming/LibreOffice/4/user/basic/script.xlc")
    
    copy_file("//lhfwad01/Users/japril/Desktop/CopyFileUtility/User/dialog_dev.xlc",
              "C:/Users/japril/AppData/Roaming/LibreOffice/4/user/basic/dialog.xlc")

    copy_file("//lhfwad01/Users/japril/Desktop/CopyFileUtility/Shared/script_dev.xlc",
              "C:/Program Files (x86)/LibreOffice/share/basic/script.xlc")
    
    copy_file("//lhfwad01/Users/japril/Desktop/CopyFileUtility/Shared/dialog_dev.xlc",
              "C:/Program Files (x86)/LibreOffice/share/basic/dialog.xlc")
    exit(0)


root = tk.Tk()
root.grid()


push_btn = tk.Button(root, text="Push Code", command=push_code).grid(column=0, row=0, padx=50, pady=50)
pull_btn = tk.Button(root, text="Pull Code", command=pull_code).grid(column=1, row=0, padx=50, pady=50)

root.title("Kinetics Macro")
root.resizable(True, True)
root.mainloop()
