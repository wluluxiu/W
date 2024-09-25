mergeInto(LibraryManager.library, {

  Compress: function (data, length) {

    var bytes = new Uint8Array(Module.HEAPU8.buffer, data, length);
    var zip = new JSZip();
    zip.file('file.png', bytes);
    zip.generateAsync({type: 'uint8array'}).then(function (compressedData) {

    });
  },

  Decompress: function (data, length) {

    var compressedData = new Uint8Array(Module.HEAPU8.buffer, data, length);
    var zip = new JSZip();

    zip.loadAsync(compressedData).then(function (zip) {

      var filename = Object.keys(zip.files)[0];
      zip.files[filename].async('uint8array').then(function (fileData) {

      });
    });
  },
});