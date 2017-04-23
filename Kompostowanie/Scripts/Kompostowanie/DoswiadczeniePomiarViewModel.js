function PomiarViewModel(model, doswiadczeniePomiar) {
    var self = this;

    this.doswiadczeniePomiar = ko.observable(doswiadczeniePomiar);

    this.pryzmaId = ko.observable(model.pryzmaId);
    this.pryzmaName = ko.observable(model.pryzmaName);
    this.typ = ko.observable(model.typ);
    this.value = ko.observable(model.value);
    this.typName = ko.observable(model.typName);

    this.minValue = ko.computed(function() {
        var typ = self.typ();
        if (typ === "Temperatura")
            return 15;
        return 0;
    });

    this.maxValue = ko.computed(function () {
        var typ = self.typ();
        if (typ === "Temperatura")
            return 90;
        if (typ === "O2")
            return 25;
        if (typ === "Nh3")
            return 1000;
        if (typ === "Co2")
            return 30;
        if (typ === "Ch4")
            return 50;
        if (typ === "H2S")
            return 10000;

        return 99999999999;
    });

    this.valueDisplayModel = ko.computed({
        read: function () {
            if (!self.value() && self.value() !== 0)
                return "";

            return parse(self.value()).toFixed(2);
        },
        write: function (value) {
            if (self.typ() !== "StanLicznikow") {
                self.value(-1);
                self.value(parse(value));
            }
            else {
                var stanLicznikaValue = parse(value);

                self.value(stanLicznikaValue);

                var przeplyw = Enumerable.From(self.doswiadczeniePomiar().przeplywy())
                    .Single(function(p) { return p.pryzmaId() == self.pryzmaId(); });

                var first = self.doswiadczeniePomiar().first();

                if (first == null)
                    przeplyw.value(0);
                else {
                    var firstStanLicznika = Enumerable.From(first.stanyLicznikow())
                        .Single(function (p) { return p.pryzmaId() == self.pryzmaId(); });

                    przeplyw.value((stanLicznikaValue - firstStanLicznika.value()) / ((self.doswiadczeniePomiar().czasComputed() - first.czasComputed()) * 60));
                }

            }
        },
        owner: self
    });

    function parse(value) {
        if (isNaN(value))
            return "";

        var val = parseFloat(value);
        if (val > self.maxValue())
            val = self.maxValue();
        if (val < self.minValue())
            val = self.minValue();

        return val;
    }

    this.isValid = ko.computed(function() {
        return !!self.value() || self.value() === 0;
    });
}

function DoswiadczeniePomiarViewModel(model, first) {
    var self = this;

    this.first = ko.observable(first);

    this.dataStart = ko.observable(new Date(model.dataStart));
    this.data = ko.observable(model.data);

    this.dzien = ko.computed(function() {
        var dataDate = new Date(self.data());
        var dataStart = self.dataStart();

        var timeDiff = dataDate.getTime() - dataStart.getTime();
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));

        return diffDays;
    });

    this.godzina = ko.observable(model.godzina);

    var i;
    this.pomiary = ko.observableArray(null);
    var tmp = [];
    for (i in model.pomiary || []) {
        var pomiar = model.pomiary[i];
        tmp.push(new PomiarViewModel(pomiar, self));
    }
    this.pomiary(tmp);

    this.stanyLicznikow = ko.observableArray(null);
    tmp = [];
    for (i in model.stanyLicznikow || []) {
        var stanLicznikow = model.stanyLicznikow[i];
        tmp.push(new PomiarViewModel(stanLicznikow, self));
    }
    this.stanyLicznikow(tmp);

    this.przeplywy = ko.observableArray(null);
    tmp = [];
    for (i in model.przeplywy || []) {
        var przeplyw = model.przeplywy[i];
        tmp.push(new PomiarViewModel(przeplyw, self));
    }
    this.przeplywy(tmp);

    this.pomiaryGrouped = ko.computed(function() {
        return groupPomiary(self.pomiary());
    });

    this.stanyLicznikowGrouped = ko.computed(function () {
        return groupPomiary(self.stanyLicznikow());
    });

    this.przeplywyGrouped = ko.computed(function () {
        return groupPomiary(self.przeplywy());
    });

    function groupPomiary(pomiary) {
        var groups = Enumerable.From(pomiary)
            .GroupBy(function (p) { return p.typ(); })
            .ToArray();

        return Enumerable.From(groups).Select(function (g) { return g.ToArray() }).ToArray();
    }

    this.dataDisplayModel = ko.computed({
        read: function () {
            if (!self.data())
                return "";
    
            return self.data();
        },
        write: function (value) {
            self.data(value);
        },
        owner: self
    });

    this.dzienDisplayModel = ko.computed(function () {
        return self.dzien();
    });

    this.godzinaDisplayModel = ko.computed({
        read: function () {
            if (!self.godzina())
                return "";
    
            return parse(self.godzina()).toFixed(2);
        },
        write: function (value) {
            self.godzina(parse(value));

            Enumerable.From(self.stanyLicznikow()).ForEach(function (p) { return p.valueDisplayModel(p.value()) });

        },
        owner: self
    });

    this.czasComputed = ko.computed(function () {
        return (self.dzien() * 24) + self.godzina() - (self.first() != null ? self.first().godzina() : self.godzina());
    });

    this.dzienComputed = ko.computed(function () {
        if (self.czasComputed() == 0) {
            return self.godzina() / 24;
        }

        return ((self.first() != null ? self.first().godzina() : 0) + self.czasComputed()) / 24;
    });

    this.czasComputedDisplayModel = ko.computed(function () {
        return self.czasComputed().toFixed(2);
    });

    this.dzienComputedDisplayModel = ko.computed(function () {
        return self.dzienComputed().toFixed(2);
    });


    this.isValid = ko.computed(function () {
        var pomiaryValid = Enumerable.From(self.pomiary()).All(function (p) { return p.isValid() });
        var stanyLicznikowValid = Enumerable.From(self.stanyLicznikow()).All(function (p) { return p.isValid() });

        return pomiaryValid && stanyLicznikowValid && !!self.data() && !!self.godzina();
    });

    function parse(value) {
        if (isNaN(value))
            return "";
    
        return parseFloat(value);
    }
}