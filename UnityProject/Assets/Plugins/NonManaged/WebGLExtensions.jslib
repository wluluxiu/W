mergeInto(LibraryManager.library, {

  OpenURL: function (url, target) {
    window.open(UTF8ToString(url), UTF8ToString(target));
  },

  SyncFileToIndexedDB: function() {
    // FS.syncfs(false, function(error) {
    //   if (error) {
    //     console.error('Error syncing file system to IndexedDB:', error);
    //     {{{ makeDynCall('vi', 'callback') }}}(1);
    //   } else {
    //     console.log('File system synced to IndexedDB successfully.');
    //     {{{ makeDynCall('vi', 'callback') }}}(0);
    //   }
    // });
    _JS_FileSystem_Sync();
    console.log('File system synced to IndexedDB successfully.');
  },
  
  SyncFileFromIndexedDB: function(callback) {
    FS.syncfs(true, function(error) {
      if (error) {
        console.error('Error syncing file system from IndexedDB:', error);
        {{{ makeDynCall('vi', 'callback') }}}(1);
      } else {
        console.log('File system synced from IndexedDB successfully.');
        {{{ makeDynCall('vi', 'callback') }}}(0);
      }
    });
  },

  JSReadFile: function(path) {
    var data = FS.readFile(UTF8ToString(path), { encoding: 'binary' });
    var length = data.length;
    var ptr = Module._malloc(length);
    HEAPU8.set(data, ptr);
    return ptr;
  },

  JSWriteFile: function(path, data, length) {
    var buffer = new Uint8Array(length);
    for (var i = 0; i < length; i++) {
      buffer[i] = HEAPU8[data + i];
    }
    console.log(UTF8ToString(path))
    FS.writeFile(UTF8ToString(path), buffer, { encoding: 'binary' });
  },
  
  JSCreateFile: function(path, name, dataPtr, dataLength) {
    try {
      var parent = UTF8ToString(path);
      var fileName = UTF8ToString(name);
      var data = new Uint8Array(Module.HEAPU8.buffer, dataPtr, dataLength);
      FS.createDataFile(parent, fileName, data, true, true, false);
      console.log('Directory created successfully!');
    } catch (e) {
      console.error('Error creating directory:', e);
    }
  },

  JSCreateDirectory: function(path) {
    try {
      FS.mkdir(UTF8ToString(path));
      console.log('Directory created successfully!');
    } catch (e) {
      console.error('Error creating directory:', e);
    }
  },

  JSRemoveFile:function(path) {
    FS.unlink(UTF8ToString(path));
  },

  JSRemoveDirectory: function(dir) {
    console.log(dir);
    var path = UTF8ToString(dir)
    console.log(path);
    var files = FS.readdir(path);

    // Iterate over each entry
    files.forEach(function(file) {
        if (file !== '.' && file !== '..') { // Ignore the special entries
            var fullPath = PATH.join(path, file);
            console.error(fullPath);
            var stat = FS.stat(fullPath);
            // If the entry is a directory, recurse. If it's a file, delete it.
            if (FS.isDir(stat.mode)) {
                _JSRemoveDirectory(allocateUTF8OnStack(fullPath));
            } else if (FS.isFile(stat.mode)) {
                FS.unlink(fullPath);
            }
        }
    });
    FS.rmdir(path);
  },

  JSMoveFile: function(oldpath, newpath) {
    var oldP = UTF8ToString(oldpath);
    var newP = UTF8ToString(newpath);
    FS.rename(oldP, newP);
  },

  JSMoveFiles: function(srcDir, destDir) {
    var srcPath = UTF8ToString(srcDir);
    var destPath = UTF8ToString(destDir);

    try {
        var destDirStat = FS.stat(destPath);
        if (!FS.isDir(destDirStat.mode)) {
            console.log('Destination path is not a directory');
            return;
        }
    } catch (e) {
        FS.mkdir(destPath);
    }

    var files = FS.readdir(srcPath);
    files.forEach(function(file) {
        if (file !== '.' && file !== '..') {
            var oldP = PATH.join(srcPath, file);
            var newP = PATH.join(destPath, file);
            var stat = FS.stat(oldP);
            if (FS.isFile(stat.mode)) {
                try {
                    var destStat = FS.stat(newP);
                    if (FS.isFile(destStat.mode)) {
                        FS.unlink(newP);
                    }
                } catch (e) {
       
                }
                FS.rename(oldP, newP);
            } else if (FS.isDir(stat.mode)) {
                _JSMoveFiles(allocateUTF8OnStack(oldP), allocateUTF8OnStack(newP));
            }
        }
    });
  },

  JSIsFileExists: function(path) {
    try{
      var pathString = UTF8ToString(path);
      var stat = FS.stat(pathString);
      if (FS.isFile(stat.mode)) {
        return true;
      }
      return false;
    }
    catch (e) {
      console.error('Error accessing the path: ' + pathString, e);
      // Handle the error or re-throw
      return false;
    }

  },

  JSGetFileLength: function(path) {
    var data = FS.readFile(UTF8ToString(path), { encoding: 'binary' });
    return data.length;
  },

  JSIsDirectoryExists: function(path) {
    try{
      var pathString = UTF8ToString(path);
      var stat = FS.stat(pathString);
      if (FS.isDir(stat.mode)) {
        return true;
      }
      return false;
    }
    catch (e) {
      console.error('Error accessing the path: ' + pathString, e);
      // Handle the error or re-throw
      return false;
    }
  },

  JSOpenFileStream: function(pathPtr, modePtr) {
    var filePath = UTF8ToString(pathPtr);
    var flags = UTF8ToString(modePtr);

    try {
      var stream = FS.open(filePath, flags);
      return stream.fd;
    } 
    catch (e) 
    {
      console.error('Error opening file:', e);
      return 0;
    }
  },

  JSCloseFileStream: function(fd) {
    try {
      FS.close(FS.getStream(fd));
      return 1; // success
    } catch (e) {
      console.log('FS.close error:', e);
      return 0; // indication of an error
    }
  },

  JSReadFileStream: function(fd, length, offset) {
    var buffer = _malloc(length);
    var stream = FS.getStream(fd);
    var position = FS.llseek(stream, offset, 0);
    var bytesRead = FS.read(stream, HEAPU8, buffer, length, position);
    // If the read operation was successful, return the pointer to the data
    if(bytesRead >= 0) {
      return buffer;
    } else {
      // If there was an error, free the allocated memory and return 0
      _free(buffer);
      return 0;
    }
  },

  JSWriteFileStream: function(fd, bufferPtr, count, offset) {
    try {
      var stream = FS.getStream(fd);
      var position = FS.llseek(stream, offset, 0);
      var data = new Uint8Array(HEAPU8.subarray(bufferPtr, bufferPtr + count));
      var bytesWritten = FS.write(stream, data, 0, count, position);
      return bytesWritten; // return the number of bytes written
    } catch (e) {
      console.log('FS.write error:', e);
      return -1; // indication of an error
    }
  },

  JSFreeAllocatedMemory: function(buffer) {
    _free(buffer);
  },
});