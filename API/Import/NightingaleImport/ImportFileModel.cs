using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightingaleImport
{
    public class ImportFileModel
    {
        private string _filename;
        private  int _colFirstN = 0;
        private  int _colLastN = 1;
        private  int _colMiddleN = 2;
        private  int _colSuff = 3;
        private  int _colPrefN = 4;
        private  int _colGen = 5;
        private  int _colDB = 6;
        private  int _colSysID = 7;
        private  int _colBkgrnd = 8;
        private  int _colTimeZ = 9;
        private  int _colPh1 = 10;
        private  int _colPh1Pref = 11;
        private  int _colPh1Type = 12;
        private  int _colPh2 = 13;
        private  int _colPh2Pref = 14;
        private  int _colPh2Type = 15;
        private  int _colEm1 = 16;
        private  int _colEm1Pref = 17;
        private  int _colEm1Type = 18;
        private  int _colEm2 = 19;
        private  int _colEm2Pref = 20;
        private  int _colEm2Type = 21;
        private  int _colAdd1L1 = 22;
        private  int _colAdd1L2 = 23;
        private  int _colAdd1L3 = 24;
        private  int _colAdd1City = 25;
        private  int _colAdd1St = 26;
        private  int _colAdd1Zip = 27;
        private  int _colAdd1Pref = 28;
        private  int _colAdd1Type = 29;
        private  int _colAdd2L1 = 30;
        private  int _colAdd2L2 = 31;
        private  int _colAdd2L3 = 32;
        private  int _colAdd2City = 33;
        private  int _colAdd2St = 34;
        private  int _colAdd2Zip = 35;
        private  int _colAdd2Pref = 36;
        private  int _colAdd2Type = 37;
        private  int _colCMan = 38;
        private  int _colSysNm = 39;
        private int _colSysPrim = 40;
        private static int _colActivateDeactivate = 41;

        public int colTimeZ
        {
            get { return _colTimeZ; }
            set { _colTimeZ = value; }
        }

        public int colPh1
        {
            get { return _colPh1; }
            set { _colPh1 = value; }
        }

        public int colPh1Pref
        {
            get { return _colPh1Pref; }
            set { _colPh1Pref = value; }
        }

        public int colPh1Type
        {
            get { return _colPh1Type; }
            set { _colPh1Type = value; }
        }

        public int colPh2
        {
            get { return _colPh2; }
            set { _colPh2 = value; }
        }

        public int colPh2Pref
        {
            get { return _colPh2Pref; }
            set { _colPh2Pref = value; }
        }

        public int colPh2Type
        {
            get { return _colPh2Type; }
            set { _colPh2Type = value; }
        }

        public int colEm1
        {
            get { return _colEm1; }
            set { _colEm1 = value; }
        }

        public int colEm1Pref
        {
            get { return _colEm1Pref; }
            set { _colEm1Pref = value; }
        }

        public int colEm1Type
        {
            get { return _colEm1Type; }
            set { _colEm1Type = value; }
        }

            public int colEm2Pref
        {
            get { return _colEm2Pref; }
            set { _colEm2Pref = value; }
        }
        public int colEm2Type
        {
            get { return _colEm2Type; }
            set { _colEm2Type = value; }
        }
        public int colAdd1L1
        {
            get { return _colAdd1L1; }
            set { _colAdd1L1 = value; }
        }
        public int colAdd1L2
        {
            get { return _colAdd1L2; }
            set { _colAdd1L2 = value; }
        }
        public int colAdd1L3
        {
            get { return _colAdd1L3; }
            set { _colAdd1L3 = value; }
        }

        public int colAdd1City
        {
            get { return _colAdd1City; }
            set { _colAdd1City = value; }
        }
        public int colAdd1St
        {
            get { return _colAdd1St; }
            set { _colAdd1St = value; }
        }
        public int colAdd1Zip
        {
            get { return _colAdd1Zip; }
            set { _colAdd1Zip = value; }
        }
        public int colAdd1Pref
        {
            get { return _colAdd1Pref; }
            set { _colAdd1Pref = value; }
        }
        public int colAdd1Type
        {
            get { return _colAdd1Type; }
            set { _colAdd1Type = value; }
        }

        public int colEm2
        {
            get { return _colEm2; }
            set { _colEm2 = value; }
        }
        public int colAdd2L1
        {
            get { return _colAdd2L1; }
            set { _colAdd2L1 = value; }
        }
        public int colAdd2L2
        {
            get { return _colAdd2L2; }
            set { _colAdd2L2 = value; }
        }
        public int colAdd2L3
        {
            get { return _colAdd2L3; }
            set { _colAdd2L3 = value; }
        }
        public int colAdd2City
        {
            get { return _colAdd2City; }
            set { _colAdd2City = value; }
        }
        public int colAdd2St
        {
            get { return _colAdd2St; }
            set { _colAdd2St = value; }
        }
        public int colAdd2Zip
        {
            get { return _colAdd2Zip; }
            set { _colAdd2Zip = value; }
        }
        public int colAdd2Pref
        {
            get { return _colAdd2Pref; }
            set { _colAdd2Pref = value; }
        }
        public int colAdd2Type
        {
            get { return _colAdd2Type; }
            set { _colAdd2Type = value; }
        }
        public int colSysPrim
        {
            get { return _colSysPrim; }
            set { _colSysPrim = value; }
        }
        public int colActivateDeactivate
        {
            get { return _colActivateDeactivate; }
            set { _colActivateDeactivate = value; }
        }

        public int colFirstN
        {
            get{return _colFirstN;}
            set{_colFirstN = value;}
        }
        public int colLastN
        {
            get { return _colLastN; }
            set { _colLastN = value; }
        }
        public int colDB
        {
            get { return _colDB; }
            set { _colDB = value; }
        }
        public int colMiddleN
        {
            get { return _colMiddleN; }
            set { _colMiddleN = value; }
        }
        public int colPrefN
        {
            get { return _colPrefN; }
            set { _colPrefN = value; }
        }
        public int colGen
        {
            get { return _colGen; }
            set { _colGen = value; }
        }
        public int colSuff
        {
            get { return _colSuff; }
            set { _colSuff = value; }
        }
        public int colBkgrnd
        {
            get { return _colBkgrnd; }
            set { _colBkgrnd = value; }
        }
        public int colCMan
        {
            get { return _colCMan; }
            set { _colCMan = value; }
        }

        public string filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

    }
}
