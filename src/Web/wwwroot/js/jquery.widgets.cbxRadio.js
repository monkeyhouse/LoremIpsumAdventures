(function ($) {

    $.widget("my.cbxRadio", { //nullable radio

        options: {
            value: '',
            defaultValue: '-1',
            activeClass: 'active',
        },

        _create: function () {

            this.group = $(this.element[0]);
            this.group.addClass('cbxRadio');

            var bindName = this.group.find('input[type=checkbox]')[0].name;

            this._appendNullInput(bindName);

            this.inputs = this.group.find('input[type=checkbox]');
            this._initChecked();
            this._bindChange();
        },

        _appendNullInput: function (bindName) {

            var nullInput = $("<input type=checkbox class=nvl />")
                            .attr('name', bindName)
                            .attr('value', this.options.defaultValue);

            this.group.append(nullInput);
        },

        _initChecked: function () {
            //add active class to checked one
            var selected = this.inputs.filter(function () { return this.checked });

            if (selected.length == 1) {
                this.options.value = selected[0].value;
                selected[0].parentNode.classList.add(this.options.activeClass);
            }

            if (selected.length == 0) {
                this.options.value = this.options.defaultValue;
                var nullInput = this._getNullInput();
                nullInput.checked = true;
            }
        },

        _getNullInput: function () {
            var that = this;
            return this.inputs.filter(function () { return this.value == that.options.defaultValue })[0];
        },

        _bindChange: function () {
            var that = this;
            this.inputs.on('change', function (e) {

                var isChecked = this.checked;
                var thisVal = this.value;

                that._setSelected(thisVal, isChecked);

                that._trigger("change");
            });

        },

        value: function (value) {
            if (value === undefined) {
                return this.options.value;
            } else {
                this._setSelected(value, true);
            }
        },

        _setSelected: function (value, isChecked) {
            var that = this;

            that.options.value = this.options.defaultValue;
            this.inputs.each(function () {

                //unselect others
                if (this.value != value && this.value) {
                    this.checked = false;
                    this.parentNode.classList.remove(that.options.activeClass);
                }

                if (this.value == value) {
                    if (isChecked) {
                        this.parentNode.classList.add(that.options.activeClass);
                        that.options.value = value;
                    } else {
                        this.parentNode.classList.remove(that.options.activeClass);
                    }
                }
            });

            if (!isChecked) {
                that.options.value = that.options.defaultValue;
                that._getNullInput().checked = true;
            }

        }

    });

    /* 
    <div class='cb-group'>
        <label><input type="checkbox" value='a' checked="checked" name="setOne">Like</label>
        <label><input type="checkbox" value='b' name="setOne">Dislike</label>
    </div>*/

    //var cbxs = [];
    ////apply to each
    //var cb = $('.checkboxes').cbxRadio({defaultValue:'z');
    //cb.on("cbxradiochange", function () {        
    //    var value = $(cb).cbxRadio("value");
    //    console.log(value);
    //});


})(jQuery);
