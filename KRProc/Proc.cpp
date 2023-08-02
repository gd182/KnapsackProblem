#include <string>
#include <windows.h>
#define Proc _declspec(dllexport)

extern "C" {
    Proc struct item {
        int Weiht;
        double Coast;
        double SpecCoast;
        int NumItem;
    };

    Proc struct itemDP {
        int Weiht;
        double Coast;
        int NumItem;
    };

    Proc struct OutInf {
        int Weiht;
        double Coast;
    };
    struct DPInfCol {
        int Weiht;
        double Coast;
        std::wstring Items;
    };

    Proc BSTR  Alg(item *ArrItem, int Count, int MaxW, OutInf *Out) {
        Out->Weiht = 0;
        Out->Coast = 0;
        std::wstring SOutItem;
        for (int i = Count - 1; i >= 0; i--) {
            if (Out->Weiht + ArrItem[i].Weiht <= MaxW) {
                Out->Weiht += ArrItem[i].Weiht;
                Out->Coast += ArrItem[i].Coast;
                if (SOutItem == L"")
                    SOutItem = std::to_wstring(ArrItem[i].NumItem);
                else
                    SOutItem += L", " + std::to_wstring(ArrItem[i].NumItem);
                if (Out->Weiht == MaxW)
                    break;
            }
        }
        return SysAllocString(SOutItem.c_str());
    }

    Proc void Sort(item *ArrItem, int Count) {
        for (int i = 1; i < Count; i++) {
            if (ArrItem[i].SpecCoast < ArrItem[i - 1].SpecCoast) {
                item buf = ArrItem[i];
                int j = i - 1;
                while (j >= 0 && buf.SpecCoast < ArrItem[j].SpecCoast) {
                    ArrItem[j + 1] = ArrItem[j];
                    j--;
                }
                ArrItem[j + 1] = buf;
            }
        }
    }

    Proc BSTR AlgDP(itemDP *ArrItem, int Count, int MaxW, OutInf *Out) {
        DPInfCol**TableDM=new DPInfCol *[Count +1];
        for (int i = 0; i <= Count; i++) {
            TableDM[i] = new DPInfCol[MaxW + 1];
            for (int j = 0; j <= MaxW; j++) {
                if (i == 0 || j == 0) {
                    TableDM[i][j].Coast = 0;
                    TableDM[i][j].Weiht = 0;
                    TableDM[i][j].Items = L"";
                    continue;
                }
                if (i == 1) {
                    if (ArrItem[0].Weiht > j) {
                        TableDM[i][j].Coast = 0;
                        TableDM[i][j].Weiht = 0;
                        TableDM[i][j].Items = L"";
                    }
                    else {
                        TableDM[i][j].Coast = ArrItem[0].Coast;
                        TableDM[i][j].Weiht = ArrItem[0].Weiht;
                        TableDM[i][j].Items = std::to_wstring(ArrItem[0].NumItem);
                    }
                    //TableDM[i][j].Coast = ArrItem[0].Weiht< j?0: ArrItem[0].Coast;
                    continue;
                }
                if (ArrItem[i - 1].Weiht > j)
                    TableDM[i][j] = TableDM[i - 1][j];
                else {
                    DPInfCol buf;
                    buf.Coast = ArrItem[i - 1].Coast + TableDM[i - 1][j - ArrItem[i - 1].Weiht].Coast;
                    buf.Weiht = ArrItem[i - 1].Weiht + TableDM[i - 1][j - ArrItem[i - 1].Weiht].Weiht;
                    buf.Items = std::to_wstring(ArrItem[i - 1].NumItem)+ L"," + TableDM[i - 1][j - ArrItem[i - 1].Weiht].Items;
                    if (ArrItem[i - 1].Coast > buf.Coast) {
                        TableDM[i][j].Coast = ArrItem[i - 1].Coast;
                        TableDM[i][j].Weiht = ArrItem[i - 1].Weiht;
                        TableDM[i][j].Items = std::to_wstring(ArrItem[i - 1].NumItem);
                    }
                    else {
                        TableDM[i][j] = buf;
                    }
                }
            }
        }
        Out->Coast = 0;
        std::wstring SOutItem;
        for (int i = 0; i <= Count; i++) {
            if (TableDM[i][MaxW].Coast > Out->Coast) {
                Out->Coast = TableDM[i][MaxW].Coast;
                Out->Weiht = TableDM[i][MaxW].Weiht;
                SOutItem = TableDM[i][MaxW].Items;
            }
        }
        return SysAllocString(SOutItem.c_str());
    }
}