import xlrd
import json
import codecs
import os


def ExcelToJson():
    dirpath = os.listdir(os.path.abspath(os.curdir))
    result = {}
    tab = "Excel列表 " + '\n'
    num = 0
    for file_path in dirpath:
        if(".xlsx" in file_path):
            if get_data(file_path) is not None:
                book = get_data(file_path)
                worksheets = book.sheet_names()
                for sheet in worksheets:
                    # 记录列表
                    num = num + 1
                    tab = tab + ('%s,%s' % (num, sheet))
                    tab = tab + '\n'
                    table = book.sheet_by_index(worksheets.index(sheet))
                    # 单表行列
                    row_0 = table.row(0)
                    nrows = table.nrows
                    ncols = table.ncols
                    # 记录Json
                    result["num"] = num
                    result[str(sheet)] = []
                    for i in range(nrows):
                        if i == 0:
                            continue
                        tmp = {}
                        for j in range(ncols):
                            title_de = str(row_0[j])
                            title_cn = title_de.split("'")[1]
                            tmp[title_cn] = table.row_values(i)[j]
                        result[str(sheet)].append(tmp)
    json_data = json.dumps(result, indent=4, sort_keys=True)
    saveFile(os.getcwd(), "Excel列表", tab, ".txt")
    saveFile(os.getcwd(), "JsonData", json_data, ".json")
    print(json_data)
    return json_data


# 获取excel数据源
def get_data(file_path):
    """获取excel数据源"""
    try:
        data = xlrd.open_workbook(file_path)
        return data
    except Exception:
        print('excel表格读取失败')
        return None


def saveFile(file_path, file_name, data, pre):
    output = codecs.open(file_path + "/" + file_name + pre, 'w', "utf-8")
    output.write(data)
    output.close()


if __name__ == '__main__':
    jsdata = ExcelToJson()
