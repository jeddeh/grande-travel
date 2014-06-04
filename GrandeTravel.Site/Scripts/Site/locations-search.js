$(function () {
    var $container = $('#gallery_container');

    // Isotope initialisation
    $container.imagesLoaded(function () {
        $container.isotope({
            itemSelector: '.package-image',
            masonry: {
                columnWidth: 100
            }
        });
    });

    // Store all the package locations in an array
    var locations = [];

    // States
    $(".state").each(function (index) {
        var state = this.innerHTML;
        state = getFullStateName(state);
        var duplicate = false;

        for (var l in locations) {
            if (locations[l].value === state) {
                duplicate = true;
                break;
            }
        }

        if (!duplicate) {
            locations.push({ "value": state });
        }
    });

    // Cities
    $(".city").each(function (index) {
        var city = this.innerHTML;
        var duplicate = false;

        for (var l in locations) {
            if (locations[l].value === city) {
                duplicate = true;
                break;
            }
        }

        if (!duplicate) {
            locations.push({ "value": city });
        }
    });

    // Typeahead function matching package locations with user input
    var matcher = function (locations) {
        return function findMatches(inputText, cb) {
            var matches = [];
            var regex = new RegExp("^" + inputText, 'i');

            $.each(locations, function (index, str) {
                if (regex.test(str.value)) {
                    matches.push(str);
                }
            });
            cb(matches);
        };
    };

    // Typeahead listener - needed to alter Cerulean bootstrap theme in order to enable hint 
    $("#txtSearch").typeahead({
        hint: false,
        highlight: false,
        minLength: 1
    }, {
        name: 'locations',
        displayKey: 'value',
        source: matcher(locations)
    });

    // Click event for dropdown items on the typeahead suggestions
    $('#txtSearch').on('typeahead:selected', function (e, datum) {
        var location = datum.value;
        filterElements(location, false);
    });

    // Click event handler fot the Search button
    $("#btnSearch").click(function () {
        var location = getFullStateName($("#txtSearch").val().trim());
        if (location) {
            filterElements(location, true);
        }
    });

    // Click event handler for All Locations button
    $("#btnAllLocations").click(function () {
        $("#filterHeading").html("Showing all locations");
        $("#txtSearch").val("");

        $("div.package-image").addClass("active-image");
        $container.isotope({ filter: ".active-image" });

        return false;
    });

    // Filters elements for isotope and displays search query heading
    function filterElements(location, usePartialText) {

        if (usePartialText) {
            $("#filterHeading").html("Showing package locations containting '" + location + "'");

        } else {
            $("#filterHeading").html("Showing packages in " + toTitleCase(location));
        }

        $("#txtSearch").val("");

        switch (toTitleCase(location)) {
            case "New South Wales": location = "NSW";
                break;
            case "Queensland": location = "QLD";
                break;
            case "Victoria": location = "VIC";
                break;
            case "Northern Territory": location = "NT";
                break;
            case "Australian Capital Territory": location = "ACT";
                break;
            case "South Australia": location = "SA";
                break;
            case "Western Australia": location = "WA";
                break;
            case "Tasmania": location = "TAS";
                break;
        }

        $("div.package-image").removeClass("active-image");

        if (usePartialText) {
            $("span.city").each(function (index) {
                if (this.innerHTML.toUpperCase().indexOf(location.toUpperCase()) > -1) {
                    $(this).closest("div").addClass("active-image");
                }
            });

            $("span.state").each(function (index) {
                if (this.innerHTML.toUpperCase().indexOf(location.toUpperCase()) > -1) {
                    $(this).closest("div").addClass("active-image");
                }
            });
        } else {
            $("span.city").each(function (index) {
                if (this.innerHTML.toUpperCase() === location.toUpperCase()) {
                    $(this).closest("div").addClass("active-image");
                }
            });

            $("span.state").each(function (index) {
                if (this.innerHTML.toUpperCase() === location.toUpperCase()) {
                    $(this).closest("div").addClass("active-image");
                }
            });
        }

        $container.isotope({ filter: ".active-image" });
        return false;
    }

    // Converts state abbreviations to full state names
    function getFullStateName(location) {

        switch (location.toUpperCase()) {
            case "NSW": location = "New South Wales";
                break;
            case "QLD": location = "Queensland";
                break;
            case "VIC": location = "Victoria";
                break;
            case "NT": location = "Northern Territory";
                break;
            case "ACT": location = "Australian Capital Territory";
                break;
            case "SA": location = "South Australia";
                break;
            case "WA": location = "Western Australia";
                break;
            case "TAS": location = "Tasmania";
                break;
        }

        return location;
    }

    // To Title Case function
    function toTitleCase(str) {
        return str.replace(/\w\S*/g, function (txt) {
            return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase();
        });
    }

    // Modal functionality

    // Image link click event handler
    $("a.package-link").click(function (event) {
        var packageId = event.currentTarget.id;

        if (isNaN(parseInt(packageId, 10)) || !isFinite(packageId)) {
            return false;
        }

        $("#packageId").val(packageId);

        var test;

        $.ajax({
            url: "/Packages/GetPackageDetails",
            data: "id=" + packageId,
            type: "GET",
            success: function (data) {
                $("#btnSubmit").show();

                $("#packageAccomodation").html("<strong>Accomodation: </strong>" + data[0].Accomodation);
                $("#packageCity").html("<strong>City: </strong>" + data[0].City);
                $("#packageState").html("<strong>State: </strong>" + data[0].State);
                $("#packagePrice").html("<strong>Price: </strong>$" + data[0].Price.toFixed(2));
                // $("#packageDescription").html("<strong>Description: </strong>" + data[0].Description);

                var activities = data[0].Activities;

                $("#activity1").html(activities[0] || "");
                $("#activity2").html(activities[1] || "");
                $("#activity3").html(activities[2] || "");

                $("#imgPackage").attr("src", data[0].ImageUrl);
            },
            error: function (xhr, status, error) {
                $("#btnSubmit").hide();
                return false;
            }
        });
        $(".modal-title").html($(this).closest(".package-link").html());
        $("#modal").modal("show");

        // TODO : Better fix for txtSearch showing a value after the image is clicked 
        $("#txtSearch").val("");
    });
});

