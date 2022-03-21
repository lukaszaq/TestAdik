using System;
using System.Collections.Generic;

namespace ProgObiektowelab3
{

    public class PozycjaNotFoundException : Exception
    {
        public PozycjaNotFoundException() : base() { }
        public PozycjaNotFoundException(string message) : base(message) { 
            Console.WriteLine($"Nie znaleziono pozycji {message}");
        }
        public PozycjaNotFoundException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    interface IZarzadzaniePozycjami
    {
        Pozycja ZnajdzPozycjePoTytule(string tytul);
        Pozycja ZnajdzPozycjePoId(int id);
        void WypiszWszystkiePozycje();
    }

    class Katalog : IZarzadzaniePozycjami
    {
        private List<Pozycja> pozycje;
        private string dzialTematyczny;
        public Katalog()
        {
            pozycje = new List<Pozycja>();
        }
        public Katalog(string dzialTematyczny)
        {
            this.dzialTematyczny = dzialTematyczny;
            pozycje = new List<Pozycja>();
        }

        public void dodajPozycje(Pozycja pozycja)
        {
            pozycje.Add(pozycja);
        }

        public void WypiszInfo()
        {
            for (int i = 0; i < pozycje.Count; i++)
            {
                pozycje[i].WypiszInfo();
            }
        }

        public Pozycja ZnajdzPozycjePoTytule(string tytul)
        {
            for (int i = 0; i < pozycje.Count; i++)
            {
                if (pozycje[i].GetTytul().Equals(tytul))
                {
                    return pozycje[i];
                }
            }
            throw new PozycjaNotFoundException();
        }

        public Pozycja ZnajdzPozycjePoId(int id)
        {
            for (int i = 0; i < pozycje.Count; i++)
            {
                if (pozycje[i].GetId().Equals(id))
                {
                    return pozycje[i];
                }
            }
            throw new PozycjaNotFoundException();
        }

        public void WypiszWszystkiePozycje()
        {
            WypiszInfo();
        }

        public string GetdzialTematyczny()
        {
            return dzialTematyczny;
        }

    }

    abstract class Pozycja
    {
        protected string tytul;
        protected int id;
        protected string Wydawnictwo;
        protected int rokWydania;

        public Pozycja()
        {

        }
        public Pozycja(string tytul, int id, string Wydawnictwo, int rokWydania)
        {
            this.tytul = tytul;
            this.id = id;
            this.Wydawnictwo = Wydawnictwo;
            this.rokWydania = rokWydania;
        }

        public virtual void WypiszInfo()
        {
            Console.WriteLine($"id: {id} tytul: {tytul} Wydawnictwo: {Wydawnictwo} Rok Wydania {rokWydania}");
        }

        public string GetTytul()
        {
            return this.tytul;
        }
        public int GetId()
        {
            return this.id;
        }
    }

    class Ksiazka : Pozycja
    {
        private int liczbaStron;
        private Autor autor;

        public Ksiazka()
        {

        }
        public Ksiazka(string tytul_, int id_, string Wydawnictwo_, int rokWydania_, int liczbaStron_) : base(tytul_, id_, Wydawnictwo_, rokWydania_)
        {
            liczbaStron = liczbaStron_;
        }
        public void dodajAutora(Autor autor)
        {
            this.autor = autor;
        }
        public override void WypiszInfo()
        {
            base.WypiszInfo();
            try {
                Console.WriteLine($"liczba stron: {liczbaStron} Autor: {autor.ToString()}");
            }
            catch (NullReferenceException e) {
                Console.WriteLine("Autor nie może być pusty!");
            }
            catch (Exception e) {
                Console.WriteLine("Inny błąd!");
            }

        }
    }

    class Autor : Osoba
    {
        private string narodowosc;

        public Autor(String imie, String nazwisko): base(imie, nazwisko)
        {}

        public Autor(string imie, string nazwisko, string narodowosc) : base(imie, nazwisko)
        {
            this.Narodowosc = narodowosc;
        }

        public string Narodowosc { get => narodowosc; set => narodowosc = value; }
    }


    class Czasopismo : Pozycja
    {
        public Czasopismo(){ }

        public Czasopismo(string tytul_, int id_, string Wydawnictwo_, int rokWydania_, int numer_) : base(tytul_, id_, Wydawnictwo_, rokWydania_)
        {
            this.Numer = numer_;
        }

        public int Numer { get; private set; }

        public override void WypiszInfo()
        {
            base.WypiszInfo();
            Console.WriteLine($"Numer czasopisma: {Numer}");
        }
    }

    class Biblioteka : IZarzadzaniePozycjami
    {
        private string adres;
        List<Katalog> katalogi;
        List<Bibliotekarz> bibliotekarze;

        public Biblioteka()
        {
            katalogi = new List<Katalog>();
            bibliotekarze = new List<Bibliotekarz>();
        }

        public Biblioteka(string adres)
        {
            this.adres = adres;
            katalogi = new List<Katalog>();
            bibliotekarze = new List<Bibliotekarz>();
        }

        public void DodajBibliotekarza(Bibliotekarz bibliotekarz)
        {
            bibliotekarze.Add(bibliotekarz);
        }

        public void WypiszBibliotekarzy()
        {
            for (int i = 0; i < bibliotekarze.Count; i++)
            {
                Console.WriteLine(bibliotekarze[i].ToString());
            }
        }

        public void DodajKatalog(Katalog katalog)
        {
            katalogi.Add(katalog);
        }

        public void DodajPozycje(Pozycja p, string dzialTematyczny)
        {
            for (int i = 0; i < katalogi.Count; i++)
            {
                if (dzialTematyczny.Equals(katalogi[i].GetdzialTematyczny()))
                {
                    katalogi[i].dodajPozycje(p);
                }
                else
                {
                    Console.WriteLine("Nieznaleziono katalogu");
                }
            }
        }

        public Pozycja ZnajdzPozycjePoTytule(string tytul)
        {
            for (int i = 0; i < katalogi.Count; i++)
            {
                return katalogi[i].ZnajdzPozycjePoTytule(tytul);
            }
            throw new PozycjaNotFoundException(tytul);
        }

        public Pozycja ZnajdzPozycjePoId(int id)
        {
            for (int i = 0; i < katalogi.Count; i++)
            {
                return katalogi[i].ZnajdzPozycjePoId(id);
            }
            throw new PozycjaNotFoundException();
        }

        public void WypiszWszystkiePozycje()
        {
            for (int i = 0; i < katalogi.Count; i++)
            {
                katalogi[i].WypiszInfo();
            }
        }
    }

    class Osoba
    {
        protected string imie;
        protected string nazwisko;

        public Osoba(string imie, string nazwisko)
        {
            this.imie = imie;
            this.nazwisko = nazwisko;
        }
        public override string ToString()
        {
            return this.imie + " " + this.nazwisko;
        }
    }

    class Bibliotekarz : Osoba
    {
        private string dataZatrudnienia;
        private double wynagrodzenie;

        public Bibliotekarz(String imie, String nazwisko): base(imie, nazwisko)
        {

        }

        public Bibliotekarz(string imie, string nazwisko, string dataZatrudnienia, double wynagrodzenie) : base(imie, nazwisko)
        {
            this.dataZatrudnienia = dataZatrudnienia;
            this.wynagrodzenie = wynagrodzenie;
        }

        public override string ToString()
        {
            return base.ToString() + dataZatrudnienia + wynagrodzenie;
        }

    }


    internal class Program
    {
        static void Main(string[] args)
        {

            Katalog k2 = new Katalog("Przygodowe");
            Ksiazka HarryPottercz1 = new Ksiazka("Harry Potter i Kamien Filozoficzny", 1, "J. P. Fantastika", 1998, 100);
            k2.dodajPozycje(HarryPottercz1);
            HarryPottercz1.dodajAutora(new Autor("J", "K", "Poslka"));
            Czasopismo cz1 = new Czasopismo("CD Action", 2, "Wydawnictwo 1", 2077, 99);
            k2.dodajPozycje(cz1);
            k2.WypiszInfo();
            Console.ReadLine();
        }
    }
}
