using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace WpfAppPuskica
{
    public class AppHelper
    {
        // ====== MODELI ======
        public class Trkac
        {
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string Email { get; set; }
        }

        public class Staza
        {
            public string Naziv { get; set; }
            public double DuzinaKm { get; set; }
        }

        public class Trka
        {
            public string Naziv { get; set; }
            public List<Trkac> Ucesnici { get; set; }
            public Staza Staza { get; set; }

            public Trka() // Konstruktor za inicijalizaciju liste
            {
                Ucesnici = new List<Trkac>();
            }
        }

        // ====== CSV HELPER ======
        public static List<string[]> ReadCsv(string path)
        {
            var result = new List<string[]>();
            if (File.Exists(path))
            {
                foreach (var line in File.ReadAllLines(path))
                {
                    result.Add(line.Split(','));
                }
            }
            return result;
        }

        public static void WriteCsv(string path, List<string[]> rows)
        {
            var lines = new List<string>();
            foreach (var r in rows)
            {
                lines.Add(string.Join(",", r));
            }
            File.WriteAllLines(path, lines);
        }

        // ====== CRUD ZA TRKAČE ======
        private static string trkaciFile = "trkaci.csv";

        public static List<Trkac> GetAllTrkaci()
        {
            var rows = ReadCsv(trkaciFile);
            var list = new List<Trkac>();
            foreach (var r in rows)
            {
                if (r.Length >= 3)
                {
                    list.Add(new Trkac
                    {
                        Ime = r[0],
                        Prezime = r[1],
                        Email = r[2]
                    });
                }
            }
            return list;
        }

        public static void SaveAllTrkaci(List<Trkac> trkaci)
        {
            var rows = new List<string[]>();
            foreach (var t in trkaci)
            {
                rows.Add(new string[] { t.Ime, t.Prezime, t.Email });
            }
            WriteCsv(trkaciFile, rows);
        }

        public static void AddTrkac(Trkac t)
        {
            var list = GetAllTrkaci();
            list.Add(t);
            SaveAllTrkaci(list);
        }

        public static void UpdateTrkac(int index, Trkac updated)
        {
            var list = GetAllTrkaci();
            if (index >= 0 && index < list.Count)
            {
                list[index] = updated;
                SaveAllTrkaci(list);
            }
        }

        public static void DeleteTrkac(int index)
        {
            var list = GetAllTrkaci();
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
                SaveAllTrkaci(list);
            }
        }

        // ====== CRUD ZA STAZE ======
        private static string stazeFile = "staze.csv";

        public static List<Staza> GetAllStaze()
        {
            var rows = ReadCsv(stazeFile);
            var list = new List<Staza>();
            foreach (var r in rows)
            {
                if (r.Length >= 2)
                {
                    double km = 0;
                    double.TryParse(r[1], out km);
                    list.Add(new Staza
                    {
                        Naziv = r[0],
                        DuzinaKm = km
                    });
                }
            }
            return list;
        }

        public static void SaveAllStaze(List<Staza> staze)
        {
            var rows = new List<string[]>();
            foreach (var s in staze)
            {
                rows.Add(new string[] { s.Naziv, s.DuzinaKm.ToString() });
            }
            WriteCsv(stazeFile, rows);
        }

        public static void AddStaza(Staza s)
        {
            var list = GetAllStaze();
            list.Add(s);
            SaveAllStaze(list);
        }

        public static void UpdateStaza(int index, Staza updated)
        {
            var list = GetAllStaze();
            if (index >= 0 && index < list.Count)
            {
                list[index] = updated;
                SaveAllStaze(list);
            }
        }

        public static void DeleteStaza(int index)
        {
            var list = GetAllStaze();
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
                SaveAllStaze(list);
            }
        }

        // ====== CRUD ZA TRKE ======
        private static string trkeFile = "trke.csv";

        public static List<Trka> GetAllTrke()
        {
            var rows = ReadCsv(trkeFile);
            var list = new List<Trka>();
            foreach (var r in rows)
            {
                if (r.Length >= 3)
                {
                    var trka = new Trka { Naziv = r[0] };
                    // Ovde možeš dodati parsiranje staze i trkača po indexu, ili samo placeholder
                    trka.Staza = new Staza { Naziv = r[1] };
                    trka.Ucesnici = new List<Trkac>(); // prazna lista, može se kasnije dodati
                    list.Add(trka);
                }
            }
            return list;
        }

        public static void SaveAllTrke(List<Trka> trke)
        {
            var rows = new List<string[]>();
            foreach (var t in trke)
            {
                rows.Add(new string[] { t.Naziv, t.Staza != null ? t.Staza.Naziv : "" });
            }
            WriteCsv(trkeFile, rows);
        }

        public static void AddTrka(Trka t)
        {
            var list = GetAllTrke();
            list.Add(t);
            SaveAllTrke(list);
        }

        public static void UpdateTrka(int index, Trka updated)
        {
            var list = GetAllTrke();
            if (index >= 0 && index < list.Count)
            {
                list[index] = updated;
                SaveAllTrke(list);
            }
        }

        public static void DeleteTrka(int index)
        {
            var list = GetAllTrke();
            if (index >= 0 && index < list.Count)
            {
                list.RemoveAt(index);
                SaveAllTrke(list);
            }
        }

        // ====== UI helper za DataGrid ======
        public static void InitDataGrid(System.Windows.Controls.DataGrid dg, string entity)
        {
            if (entity == "Trkaci")
                dg.ItemsSource = GetAllTrkaci();
            else if (entity == "Staze")
                dg.ItemsSource = GetAllStaze();
            else if (entity == "Trke")
                dg.ItemsSource = GetAllTrke();
        }

        public static void BtnDodaj_Click(System.Windows.Controls.DataGrid dg, string entity)
        {
            if (entity == "Trkaci")
            {
                var t = new Trkac { Ime = "Neko", Prezime = "Nekic", Email = "neko@mail.com" };
                AddTrkac(t);
                dg.ItemsSource = GetAllTrkaci();
            }
            else if (entity == "Staze")
            {
                var s = new Staza { Naziv = "Nova staza", DuzinaKm = 5 };
                AddStaza(s);
                dg.ItemsSource = GetAllStaze();
            }
            else if (entity == "Trke")
            {
                var t = new Trka { Naziv = "Nova trka", Staza = new Staza { Naziv = "Staza1" } };
                AddTrka(t);
                dg.ItemsSource = GetAllTrke();
            }
        }

        public static void BtnObrisi_Click(System.Windows.Controls.DataGrid dg, string entity)
        {
            int index = dg.SelectedIndex;
            if (index < 0) return;

            if (entity == "Trkaci")
            {
                DeleteTrkac(index);
                dg.ItemsSource = GetAllTrkaci();
            }
            else if (entity == "Staze")
            {
                DeleteStaza(index);
                dg.ItemsSource = GetAllStaze();
            }
            else if (entity == "Trke")
            {
                DeleteTrka(index);
                dg.ItemsSource = GetAllTrke();
            }
        }
    }
}
