function SkladnikViewModel(skladniki, model, defaultSkladnik) {
    var self = this;

    this.skladniki = skladniki;

    this.skladnik = ko.observable(null);
    this.suchaMasa = ko.observable(null);
    this.masaSwieza = ko.observable(null);
    this.zawartoscC = ko.observable(null);
    this.zawartoscN = ko.observable(null);

    this.iloscWSm = ko.computed(function () {
        return parse(self.suchaMasa()) * parse(self.masaSwieza()) / 100;
    });
    this.udzial = ko.computed(function() {
        return self.iloscWSm() / self.skladniki.iloscWSmSuma() * 100;
    });
    this.masaC = ko.computed(function () {
        return parse(self.iloscWSm()) * parse(self.zawartoscC());
    });
    this.masaN = ko.computed(function () {
        return parse(self.iloscWSm()) * parse(self.zawartoscN());
    });
    this.cn = ko.computed(function () {
        return parse(self.masaC()) / parse(self.masaN());
    });

    //------------

    this.skladnik(model.skladnik || defaultSkladnik);
    this.suchaMasa(model.suchaMasa);
    this.masaSwieza(model.masaSwieza);
    this.zawartoscC(model.zawartoscC);
    this.zawartoscN(model.zawartoscN);

    //------------

    this.suchaMasaDisplayModel = ko.computed({
        read: function () {
            if (!self.suchaMasa() && self.suchaMasa() !== 0)
                return "";

            return parse(self.suchaMasa()).toFixed(2);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0)
                    value = 0;
                if (value > 100)
                    value = 100;

                self.suchaMasa(-1);
            }

            self.suchaMasa(value);
        },
        owner: self
    });

    this.udzialDisplayModel = ko.computed(function () {
        if (!self.udzial() && self.udzial() !== 0)
            return "";

        return parse(self.udzial()).toFixed(2);
    });

    this.iloscWSmDisplayModel = ko.computed(function () {
        if (!self.iloscWSm() && self.iloscWSm() !== 0)
            return "";

        return parse(self.iloscWSm()).toFixed(2);
    });

    this.masaSwiezaDisplayModel = ko.computed({
        read: function () {
            if (!self.masaSwieza() && self.masaSwieza() !== 0)
                return "";

            return parse(self.masaSwieza()).toFixed(2);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0.1)
                    value = 0.1;
                if (value > 100)
                    value = 100;

                self.masaSwieza(-1);
            }

            self.masaSwieza(value);
        },
        owner: self
    });

    this.zawartoscCDisplayModel = ko.computed({
        read: function () {
            if (!self.zawartoscC() && self.zawartoscC() !== 0)
                return "";

            return parse(self.zawartoscC()).toFixed(2);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0)
                    value = 0;
                if (value > 1000)
                    value = 1000;

                self.zawartoscC(-1);
            }

            self.zawartoscC(value);
        },
        owner: self
    });

    this.zawartoscNDisplayModel = ko.computed({
        read: function () {
            if (!self.zawartoscN() && self.zawartoscN() !== 0)
                return "";

            return parse(self.zawartoscN()).toFixed(2);
        },
        write: function (value) {
            value = parse(value);

            if (!isNaN(value)) {
                if (value < 0)
                    value = 0;
                if (value > 1000)
                    value = 1000;

                self.zawartoscN(-1);
            }

            self.zawartoscN(value);
        },
        owner: self
    });

    this.masaCDisplayModel = ko.computed(function () {
        if (!self.masaC() && self.masaC() !== 0)
            return "";

        return parse(self.masaC()).toFixed(2);
    });

    this.masaNDisplayModel = ko.computed(function () {
        if (!self.masaN() && self.masaN() !== 0)
            return "";

        return parse(self.masaN()).toFixed(2);
    });

    this.cnDisplayModel = ko.computed(function () {
        if (!self.cn() && self.cn() !== 0)
            return "";

        return parse(self.cn()).toFixed(2);
    });

    //------------

    function parse(value) {
        if (isNaN(value))
            return "";

        return parseFloat(value);
    }
};

function SkladnikListViewModel(model, defaultSkladnik) {
    var self = this;

    this.skladniki = ko.observableArray([]);
    this.itemToAdd = ko.observable(null);

    this.add = function () {
        if (
            isNaN(self.itemToAdd().suchaMasa()) ||
            isNaN(self.itemToAdd().udzial()) ||
            isNaN(self.itemToAdd().iloscWSm()) ||
            isNaN(self.itemToAdd().masaSwieza()) ||
            isNaN(self.itemToAdd().zawartoscC()) ||
            isNaN(self.itemToAdd().zawartoscN()) ||
            isNaN(self.itemToAdd().masaC()) ||
            isNaN(self.itemToAdd().masaN()) ||
            isNaN(self.itemToAdd().cn())
            )
            return;

        self.skladniki.push(self.itemToAdd());
        self.itemToAdd(new SkladnikViewModel(self, {}, defaultSkladnik));
    }

    this.edit = function (skladnik) {
        self.skladniki.splice(self.skladniki.indexOf(skladnik), 1);
        self.itemToAdd(skladnik);
    }

    this.remove = function (skladnik) {
        self.skladniki.splice(self.skladniki.indexOf(skladnik), 1);
    }

    this.iloscWSmSuma = ko.computed(function () {
        return Enumerable.From(self.skladniki()).Sum(function (s) { return s.iloscWSm() });
    });

    this.masaSwiezaSuma = ko.computed(function () {
        return Enumerable.From(self.skladniki()).Sum(function (s) { return s.masaSwieza() });
    });

    this.suchaMasa = ko.computed(function () {
        return self.iloscWSmSuma() / self.masaSwiezaSuma() * 100;
    });

    this.wilgotnoscOstateczna = ko.computed(function () {
        return 100 - self.suchaMasa();
    });

    this.wilgotnosc = ko.computed(function () {
        return (1 - (self.iloscWSmSuma() / self.masaSwiezaSuma())) * 100;
    });

    this.udzialSuma = ko.computed(function () {
        return Enumerable.From(self.skladniki()).Sum(function (s) { return s.udzial() });
    });

    this.masaCSuma = ko.computed(function () {
        return Enumerable.From(self.skladniki()).Sum(function (s) { return s.masaC() });
    });

    this.masaNSuma = ko.computed(function () {
        return Enumerable.From(self.skladniki()).Sum(function (s) { return s.masaN() });
    });

    this.cn = ko.computed(function () {
        return self.masaCSuma() / self.masaNSuma();
    });

    this.suchaMasaDisplayModel = ko.computed(function () {
        if (!self.suchaMasa() && self.suchaMasa() !== 0)
            return "";

        return parse(self.suchaMasa()).toFixed(2);
    });

    this.wilgotnoscOstatecznaDisplayModel = ko.computed(function () {
        if (!self.wilgotnoscOstateczna() && self.wilgotnoscOstateczna() !== 0)
            return "";

        return parse(self.wilgotnoscOstateczna()).toFixed(2);
    });

    this.wilgotnoscDisplayModel = ko.computed(function () {
        if (!self.wilgotnosc() && self.wilgotnosc() !== 0)
            return "";

        return parse(self.wilgotnosc()).toFixed(2);
    });

    this.udzialSumaDisplayModel = ko.computed(function () {
        if (!self.udzialSuma() && self.udzialSuma() !== 0)
            return "";

        return parse(self.udzialSuma()).toFixed(2);
    });

    this.iloscWSmSumaDisplayModel = ko.computed(function () {
        if (!self.iloscWSmSuma() && self.iloscWSmSuma() !== 0)
            return "";

        return parse(self.iloscWSmSuma()).toFixed(2);
    });

    this.masaSwiezaSumaDisplayModel = ko.computed(function () {
        if (!self.masaSwiezaSuma() && self.masaSwiezaSuma() !== 0)
            return "";

        return parse(self.masaSwiezaSuma()).toFixed(2);
    });

    this.masaCSumaDisplayModel = ko.computed(function () {
        if (!self.masaCSuma() && self.masaCSuma() !== 0)
            return "";

        return parse(self.masaCSuma()).toFixed(2);
    });

    this.masaNSumaDisplayModel = ko.computed(function () {
        if (!self.masaNSuma() && self.masaNSuma() !== 0)
            return "";

        return parse(self.masaNSuma()).toFixed(2);
    });

    this.cnDisplayModel = ko.computed(function () {
        if (!self.cn() && self.cn() !== 0)
            return "";

        return parse(self.cn()).toFixed(2);
    });

    var tmp = [];
    for (var i in model.skladniki || []) {
        var skladnik = model.skladniki[i];
        tmp.push(new SkladnikViewModel(self, skladnik, defaultSkladnik));
    }
    this.skladniki(tmp);

    this.itemToAdd(new SkladnikViewModel(self, {}, defaultSkladnik));

    function parse(value) {
        if (isNaN(value))
            return "";

        return parseFloat(value);
    }
}