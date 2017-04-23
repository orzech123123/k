function AeracjaEntryViewModel(model) {
    var self = this;

    this.skladnikiMasa = ko.observable(model.skladnikiMasa);
    this.wilgotnosc = ko.observable(model.wilgotnosc);
    this.iloscWSmSuma = ko.observable(model.iloscWSmSuma);
    this.masaNSuma = ko.observable(model.masaNSuma);
    this.pryzmaId = ko.observable(model.pryzmaId);
    this.aeracjaId = ko.observable(model.aeracjaId);

    this.ileZebranoNaProbki = ko.observable(model.ileZebranoNaProbki);
    this.wysokoscOdBrzegu = ko.observable(model.wysokoscOdBrzegu);
    this.przeplyw = ko.observable(model.przeplyw);
    this.ph1 = ko.observable(model.ph1);
    this.ph2 = ko.observable(model.ph2);
    this.konduktywnosc1 = ko.observable(model.konduktywnosc1);
    this.konduktywnosc2 = ko.observable(model.konduktywnosc2);
    this.konduktywnosc2 = ko.observable(model.konduktywnosc2);

    this.ileZebranoNaProbkiDisplayModel = ko.computed({
        read: function () {
            if (!self.ileZebranoNaProbki() && self.ileZebranoNaProbki() !== 0)
                return "";

            return parse(self.ileZebranoNaProbki()).toFixed(2);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0.1)
                    value = 0.1;
                if (value > 2)
                    value = 2;

                self.ileZebranoNaProbki(-1);
            }

            self.ileZebranoNaProbki(value);
        },
        owner: self
    });

    this.wysokoscOdBrzeguDisplayModel = ko.computed({
        read: function () {
            if (!self.wysokoscOdBrzegu() && self.wysokoscOdBrzegu() !== 0)
                return "";

            return parse(self.wysokoscOdBrzegu()).toFixed(2);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0)
                    value = 0;
                if (value > 30)
                    value = 30;

                self.wysokoscOdBrzegu(-1);
            }

            self.wysokoscOdBrzegu(value);
        },
        owner: self
    });

    this.masaPoczatkowa = ko.computed(function () {
        return parse(self.skladnikiMasa()) - parse(self.ileZebranoNaProbki());
    });

    this.masaPoczatkowaDisplayModel = ko.computed(function () {
        if (!self.masaPoczatkowa() && self.masaPoczatkowa() !== 0)
            return "";

        return parse(self.masaPoczatkowa()).toFixed(2);
    });

    this.objetoscPoczatkowa = ko.computed(function () {
        return 5 * 5 * (7.3 - (self.wysokoscOdBrzegu() / 10));
    });

    this.objetoscPoczatkowaDisplayModel = ko.computed(function () {
        if (!self.objetoscPoczatkowa() && self.objetoscPoczatkowa() !== 0)
            return "";

        return parse(self.objetoscPoczatkowa()).toFixed(2);
    });

    this.gestosc = ko.computed(function () {
        return self.masaPoczatkowa() / self.objetoscPoczatkowa() * 1000;
    });

    this.gestoscDisplayModel = ko.computed(function () {
        if (!self.gestosc() && self.gestosc() !== 0)
            return "";

        return parse(self.gestosc()).toFixed(2);
    });

    this.przeplywDisplayModel = ko.computed({
        read: function () {
            if (!self.przeplyw() && self.przeplyw() !== 0)
                return "";

            return parse(self.przeplyw()).toFixed(2);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0.5)
                    value = 0.5;
                if (value > 20)
                    value = 20;

                self.przeplyw(-1);
            }

            self.przeplyw(value);
        },
        owner: self
    });

    this.suchaMasa = ko.computed(function () {
        return self.iloscWSmSuma() - (self.ileZebranoNaProbki() * ((100 - self.wilgotnosc()) / 100));
    });

    this.suchaMasaDisplayModel = ko.computed(function () {
        if (!self.suchaMasa() && self.suchaMasa() !== 0)
            return "";

        return parse(self.suchaMasa()).toFixed(2);
    });

    this.cOrg = ko.computed(function () {
        return self.suchaMasa() / 2;
    });

    this.cOrgDisplayModel = ko.computed(function () {
        if (!self.cOrg() && self.cOrg() !== 0)
            return "";

        return parse(self.cOrg()).toFixed(2);
    });

    this.nTot = ko.computed(function () {
        return self.masaNSuma();
    });

    this.nTotDisplayModel = ko.computed(function () {
        if (!self.nTot() && self.nTot() !== 0)
            return "";

        return parse(self.nTot()).toFixed(2);
    });

    this.ph1DisplayModel = ko.computed({
        read: function () {
            if (!self.ph1() && self.ph1() !== 0)
                return "";

            return parse(self.ph1()).toFixed(2);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0)
                    value = 0;
                if (value > 14)
                    value = 14;

                self.ph1(-1);
            }

            self.ph1(value);
        },
        owner: self
    });

    this.ph2DisplayModel = ko.computed({
        read: function () {
            if (!self.ph2() && self.ph2() !== 0)
                return "";

            return parse(self.ph2()).toFixed(2);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0)
                    value = 0;
                if (value > 14)
                    value = 14;

                self.ph2(-1);
            }

            self.ph2(value);
        },
        owner: self
    });

    this.konduktywnosc1DisplayModel = ko.computed({
        read: function () {
            if (!self.konduktywnosc1() && self.konduktywnosc1() !== 0)
                return "";

            return parse(self.konduktywnosc1()).toFixed(5);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0.001)
                    value = 0.001;
                if (value > 50)
                    value = 50;

                self.konduktywnosc1(-1);
            }

            self.konduktywnosc1(value);
        },
        owner: self
    });

    this.konduktywnosc2DisplayModel = ko.computed({
        read: function () {
            if (!self.konduktywnosc2() && self.konduktywnosc2() !== 0)
                return "";

            return parse(self.konduktywnosc2()).toFixed(5);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0.001)
                    value = 0.001;
                if (value > 50)
                    value = 50;

                self.konduktywnosc2(-1);
            }

            self.konduktywnosc2(value);
        },
        owner: self
    });

    this.phSrednia = ko.computed(function () {
        return (self.ph1() + self.ph2()) / 2;
    });

    this.phSredniaDisplayModel = ko.computed(function () {
        if (!self.phSrednia() && self.phSrednia() !== 0)
            return "";

        return parse(self.phSrednia()).toFixed(2);
    });

    this.konduktywnoscSrednia = ko.computed(function () {
        return (self.konduktywnosc1() + self.konduktywnosc2()) / 2;
    });

    this.konduktywnoscSredniaDisplayModel = ko.computed(function () {
        if (!self.konduktywnoscSrednia() && self.konduktywnoscSrednia() !== 0)
            return "";

        return parse(self.konduktywnoscSrednia()).toFixed(5);
    });

    this.isValid = ko.computed(function () {
        return !isNaN(self.ileZebranoNaProbki()) &&
            !isNaN(self.wysokoscOdBrzegu()) &&
            !isNaN(self.przeplyw()) &&
            !isNaN(self.ph1()) &&
            !isNaN(self.ph2()) &&
            !isNaN(self.konduktywnosc1()) &&
            !isNaN(self.konduktywnosc2()) &&
            !isNaN(self.masaPoczatkowa()) &&
            !isNaN(self.objetoscPoczatkowa()) &&
            !isNaN(self.gestosc()) &&
            !isNaN(self.suchaMasa()) &&
            !isNaN(self.cOrg()) &&
            !isNaN(self.nTot()) &&
            !isNaN(self.phSrednia()) &&
            !isNaN(self.konduktywnoscSrednia());
    });

    function parse(value) {
        if (isNaN(value))
            return "";

        return parseFloat(value);
    }
};

function AeracjaViewModel(model) {
    var self = this;

    this.aeracje = ko.observableArray([]);

    var tmp = [];
    for (var i in model.aeracje || []) {
        var aeracja = model.aeracje[i];
        tmp.push(new AeracjaEntryViewModel(aeracja));
    }
    this.aeracje(tmp);

    this.isValid = ko.computed(function() {
        return Enumerable.From(self.aeracje()).All(function(p) { return p.isValid() });
    });


}