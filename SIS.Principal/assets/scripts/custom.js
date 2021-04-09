var Regex = {
    Alphanumeric: /[^A-Za-zñáéíóú0-9 ]/,
    AlphanumericWoSpace: /[^A-Za-zñáéíóú0-9]/,
    Letter: /^[A-Za-z ñáéíóú]/i,
    LetterWoTilde: /[^A-Za-z ]/,
    Numeric: /[^0-9]/,
    Decimal: /^\d{0,3}$|^\d{0,2}[\.]\d{1}$/,
    EMail: /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/,
    AllCharacters: ' !#$%&\()*+,./:;<=>?@[\\]^_`{|}~-',
    AllLetters: 'abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZáéíóú'
};
var TypeValidation = {
    Alphanumeric: 'alphanumeric',
    AlphanumericWoSpace: 'alphanumeric-wo-space',
    Letter: 'letter',
    LetterWoTilde: 'letter-wo-tilde',
    Numeric: 'numeric',
    Decimal: 'decimal',
    Date: 'date',
    Hour: 'hour',
    Email: 'email',
    Limit: 'limit',
    NoRepeat: 'no-repeat'
};
var TypeMessage = {
    Primary: 'primary',
    Error: 'error',
    Success: 'success',
    Information: 'info',
    Warning: 'warning'
};
var Event = {
    KeyUp: 'keyup',
    KeyPress: 'keypress',
    KeyDown: 'keydown',
    FocusOut: 'focusout',
    FocusIn: 'focusin'
};

(function ($) {
    $("#modal-success").on('shown.bs.modal', function (e) {
        $('#modal-success .btn-success').focus();
    });

    $.fn.extend({
        Validate: function (options) {
            defaults = {

                // Tipo de validación a aplicar en el componente inicializado
                type: TypeValidation.Alphanumeric,

                // Bloqueo de fecha anterior para la validación de fecha
                blockBefore: true,

                // Bloqueo de fecha posterior para la validación de fecha
                blockAfter: true,

                // Indicación si la validación de fecha será aplicada para la fecha de nacimiento de una persona
                isPerson: false,

                // Mínimo de caracteres aceptados en el componente inicializado, para la validación de límite
                min: null,

                // Máximo de caracteres aceptados en el componente inicializado, para la validación de límite
                max: null,

                // Cantidad de caracteres que debe aceptar obligatoriamente el componente inicializado, para la validación de límite
                obligatory: null,

                // Función adicional a aplicar en el evento [focusout]
                additional: null,

                // Caracteres especiales aceptados por el componente inicializado, además de la validación ya aplicada
                special: null,

                // Inidicación si el campo permite que el componente inicializado tenga valores nulos
                nullable: true,

                // Arreglo de caracteres o caracter que no se debe repetir en el componente inicializado
                noRepeat: null,

                // Confirmación si desea mostrar el mensaje de validación o no (por defecto muestra mensaje)
                showMessage: true
            }

            var options = $.extend({}, defaults, options);

            this.each(function () {
                var component = this;
                switch (options.type) {

                    /**
                     * @author Fabián Pérez Vásquez
                     * Validación para un campo de letras y números, sin caracteres especiales
                     * @param (opcional) special Cadena de caracteres especiales los cuáles serán aceptados por el componente validado, además de sólo número
                     * @param (opcional) additional 
                     */
                    case TypeValidation.Alphanumeric:
                        $(component).off();
                        $(component).on('keydown keypress keyup', function (event) {
                            var value = $(component).val(), no_accept = "";
                            for (i = 0; i < value.length; i++) {
                                var character = value[i], found = false;
                                if (Regex.Alphanumeric.test(character)) found = true;
                                if (found) no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            var new_value = value.replace(new RegExp('[' + no_accept + ']', 'gi'), "");
                            if (!equalsIgnoreCase(value, new_value)) $(this).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional)) {
                                if (!options.additional(event, event.type)) {
                                    event.preventDefault();
                                }
                            }
                        });
                        $(component).focusout(function (event) {
                            var value = $(component).val(), no_accept = "";
                            for (i = 0; i < value.length; i++) {
                                var character = value[i], found = false;
                                if (Regex.Alphanumeric.test(character)) found = true;
                                if (found) no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            $(this).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional)) {
                                if (options.additional(event, event.type)) $(component).attr('validated', true);
                                else $(component).attr('validated', false);
                            } else {
                                if (isNull(value)) $(component).attr('validated', true);
                            }
                        });
                        break;

                    /**
                     * @author Fabián Pérez Vásquez
                     * Validación para un campo de letras y números, sin caracteres especiales (sin espacio)
                     * @param (opcional) special Cadena de caracteres especiales los cuáles serán aceptados por el componente validado, además de sólo número
                     * @param (opcional) additional 
                     */
                    case TypeValidation.AlphanumericWoSpace:
                        $(component).off();
                        $(component).on('keydown keypress keyup', function (event) {
                            var value = $(component).val(), no_accept = "";
                            for (i = 0; i < value.length; i++) {
                                var character = value[i], found = false;
                                if (Regex.AlphanumericWoSpace.test(character)) found = true;
                                if (found) no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            var new_value = value.replace(new RegExp('[' + no_accept + ']', 'gi'), "");
                            if (!equalsIgnoreCase(value, new_value)) $(this).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional)) {
                                if (!options.additional(event, event.type)) {
                                    event.preventDefault();
                                }
                            }
                        });
                        $(component).focusout(function (event) {
                            var value = $(component).val(), no_accept = "";
                            for (i = 0; i < value.length; i++) {
                                var character = value[i], found = false;
                                if (Regex.AlphanumericWoSpace.test(character)) found = true;
                                if (found) no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            $(this).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional)) {
                                if (options.additional(event, event.type)) $(component).attr('validated', true);
                                else $(component).attr('validated', false);
                            } else {
                                if (isNull(value)) $(component).attr('validated', true);
                            }
                        });
                        break;

                    /**
                     * @author Fabián Pérez Vásquez
                     * Validación para un campo de sólo letras normales (con tilde)
                     * @param (opcional) special Cadena de caracteres especiales los cuáles serán aceptados por el componente validado, además de sólo número
                     * @param (opcional) additional 
                     */
                    case TypeValidation.Letter:
                        $(component).off();
                        $(component).on('keydown keypress keyup', function (event) {
                            var value = $(component).val(), no_accept = "";
                            for (i = 0; i < value.length; i++) {
                                var character = value[i], found = false;
                                if (!Regex.Letter.test(character)) found = true;
                                if (found) no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            var new_value = value.replace(new RegExp('[' + no_accept + ']', 'gi'), "");
                            if (!equalsIgnoreCase(value, new_value)) $(this).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional)) {
                                if (!options.additional(event, event.type)) {
                                    event.preventDefault();
                                }
                            }
                        });
                        $(component).focusout(function (event) {
                            var value = $(component).val(), no_accept = "";
                            //if (!isNull(value)) {
                            for (i = 0; i < value.length; i++) {
                                var character = value[i];
                                var found = false;
                                if (!Regex.Letter.test(character))
                                    found = true;
                                if (found)
                                    no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            $(this).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional)) {
                                if (options.additional(event, event.type)) {
                                    $(component).attr('validated', true);
                                } else {
                                    $(component).attr('validated', false);
                                }
                            } else {
                                $(component).attr('validated', true);
                            }
                            //}
                        });
                        break;

                    /**
                     * @author Fabián Pérez Vásquez
                     * Validación para un campo de sólo letras normales (sin tilde)
                     * @param (opcional) special Cadena de caracteres especiales los cuáles serán aceptados por el componente validado, además de sólo número
                     * @param (opcional) additional Función a ejecutar en el evento [focusout]
                     */
                    case TypeValidation.LetterWoTilde:
                        $(component).off();
                        $(component).on('keydown keypress keyup', function (event) {
                            var value = $(component).val(), no_accept = "";
                            for (i = 0; i < value.length; i++) {
                                var character = value[i];
                                var found = false;
                                if (Regex.LetterWoTilde.test(character)) found = true;
                                if (found) no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            var new_value = value.replace(new RegExp('[' + no_accept + ']', 'gi'), "");
                            if (!equalsIgnoreCase(value, new_value)) $(this).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional)) {
                                if (!options.additional(event, event.type)) {
                                    event.preventDefault();
                                }
                            }
                        });
                        $(component).focusout(function (event) {
                            var value = $(component).val(), no_accept = "";
                            for (i = 0; i < value.length; i++) {
                                var character = value[i], found = false;
                                if (Regex.LetterWoTilde.test(character)) found = true;
                                if (found) no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            $(this).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional)) {
                                if (options.additional(event, event.type)) $(component).attr('validated', true);
                                else $(component).attr('validated', false);
                            } else {
                                if (isNull(value)) $(component).attr('validated', true);
                            }
                        });
                        break;

                    /**
                     * @author Fabián Pérez Vásquez
                     * Validación para un campo numérico
                     * @param (opcional) special Cadena de caracteres especiales los cuáles serán aceptados por el componente validado, además de sólo número
                     * @param (opcional) additional
                     */
                    case TypeValidation.Numeric:
                        $(component).off();
                        $(component).on('keydown keypress keyup', function (event) {
                            var value = $(component).val(), no_accept = "";
                            for (i = 0; i < value.length; i++) {
                                var character = value[i], found = false;
                                if (Regex.Numeric.test(character)) found = true;
                                if (found) no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            var new_value = value.replace(new RegExp('[' + no_accept + ']', 'gi'), "");
                            if (!equalsIgnoreCase(value, new_value)) $(component).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional)) {
                                if (!options.additional(event, event.type)) {
                                    event.preventDefault();
                                }
                            }
                        });
                        $(component).focusout(function (event) {
                            var value = $(component).val(), no_accept = "";
                            for (i = 0; i < value.length; i++) {
                                var character = value[i], found = false;
                                if (Regex.Numeric.test(character)) found = true;
                                if (found) no_accept += character;
                            }
                            if (!isNull(options.special)) no_accept = no_accept.replace(new RegExp('[' + options.special + ']', 'gi'), "");
                            $(component).val(value.replace(new RegExp('[' + no_accept + ']', 'gi'), ""));
                            if (!isNull(options.additional) && !isNull(value)) {
                                if (options.additional(event, event.type)) { $(component).attr('validated', true); }
                                else { $(component).attr('validated', false); }
                            } else {
                                $(component).attr('validated', true);
                            }
                        });
                        break;

                    /**
                     * @author Fabián Pérez Vásquez
                     * Validación de fecha; valida que el día sea menor a 31 y que el mes sea menor a 12.
                     * @param (opcional) blockBefore [default: false] Si se negará a que el usuario no ingrese una fecha menor a hoy
                     * @param (opcional) isPerson [default: false] Si la validación es para la fecha de nacimiento de una persona, lo cuál validará que la edad de dicha persona no sea mayor a 120 años
                     * @param (opcional) blockAfter [default: false] Si se negará a que el usuario no ingrese una fecha mayor a hoy
                     */
                    case TypeValidation.Date:
                        $(component).Validate({ type: TypeValidation.Numeric, special: '/' });
                        $(component).Validate({ type: TypeValidation.Limit, obligatory: 10, showMessage: false });
                        $(component).keydown(function (event) {
                            var value = $(component).val();
                            var iCode = event.which ? event.which : event.keyCode;
                            if ((value.length == 2 || value.length == 5) && iCode != 8) {
                                $(component).val(value + "/");
                            }
                        });
                        $(component).keyup(function (event) {
                            var value = $(component).val();
                            if (!isNull(value)) {
                                if (value.length == 2) {
                                    if (parseInt(value) > 31) {
                                        $(component).val("");
                                        General.Utils.ShowMessage(TypeMessage.Error, "El d\u00EDa ingresado es inv\u00E1lido.")
                                    }
                                } else if (value.length == 5) {
                                    var month = parseInt(value.substring(3, 5));
                                    if (month > 12) {
                                        $(this).val(value.substring(0, 2) + "/");
                                        General.Utils.ShowMessage(TypeMessage.Error, "El mes ingresado es inv\u00E1lido.")
                                    }
                                } else if (value.length == 10) {
                                    var day_in_string = value.substring(0, 2), day = parseInt(value.substring(0, 2));
                                    var month_in_string = value.substring(3, 5), month = parseInt(value.substring(3, 5));
                                    var year_in_string = value.substring(6, 10), year = parseInt(value.substring(6, 10));
                                    if (!isNull(options.blockAfter)) {
                                        if (options.blockAfter) {
                                            if (year > new Date().getFullYear()) {
                                                $(component).val(day_in_string + "/" + month_in_string + "/");
                                                event.preventDefault();
                                            }
                                            if (new Date(year, month - 1, day) > new Date()) {
                                                $(component).val("");
                                                General.Utils.ShowMessage(TypeMessage.Error, "La fecha debe ser menor o igual a la de hoy.")
                                                event.preventDefault();
                                            }
                                        }
                                    }
                                    if (!isNull(options.blockBefore)) {
                                        if (options.blockBefore) {
                                            if (year < new Date().getFullYear()) {
                                                $(component).val(day_in_string + "/" + month_in_string + "/");
                                                event.preventDefault();
                                            }
                                            if (new Date(year, month - 1, day) < new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getUTCDate())) {
                                                $(component).val("");
                                                General.Utils.ShowMessage(TypeMessage.Error, "La fecha debe ser mayor o igual a la de hoy.")
                                                event.preventDefault();
                                            }
                                        }
                                    }
                                    if (!isNull(options.isPerson)) {
                                        if (options.isPerson) {
                                            if (new Date().getFullYear() - year > 120) {
                                                $(component).val("");
                                                General.Utils.ShowMessage(TypeMessage.Error, "La persona a ingresar tiene m\u00E1s de 120 a\u00F1os.");
                                                event.preventDefault();
                                            }
                                        }
                                    }
                                    if (!(month_in_string > 0 && month_in_string < 13 && year_in_string > 1889 && year_in_string < 32768 && day_in_string > 0 && day_in_string <= (new Date(year_in_string, month_in_string, 0)).getDate())) {
                                        $(component).val("");
                                        General.Utils.ShowMessage(TypeMessage.Error, 'La fecha ingresada es inv\u00E1lida.');
                                        return false;
                                    }
                                }
                            }
                        });
                        $(component).focusout(function (event) {
                            var value = ($(component).val()).toString();
                            if (!isNull(value)) {
                                var day_in_string = value.substring(0, 2), day = parseInt(value.substring(0, 2));
                                var month_in_string = value.substring(3, 5), month = parseInt(value.substring(3, 5));
                                var year_in_string = value.substring(6, 10), year = parseInt(value.substring(6, 10));
                                if (!(month_in_string > 0 && month_in_string < 13 && year_in_string > 1889 && year_in_string < 32768 && day_in_string > 0 && day_in_string <= (new Date(year_in_string, month_in_string, 0)).getDate())) {
                                    if (options.showMessage) { General.Utils.ShowMessage(TypeMessage.Error, 'La fecha ingresada es inv\u00E1lida.'); }
                                    $(component).val("");
                                    $(component).focus();
                                    return false;
                                }
                            }
                            if (value.length >= 10) { $(component).val(value.substring(0, 10)); }
                            if (value.length == 10) { $(component).closest('.form-group').removeClass('has-error'); }
                        });
                        break;

                    case TypeValidation.Hour:
                        $(component).Validate({ type: TypeValidation.Numeric, special: ':' });
                        $(component).Validate({ type: TypeValidation.Limit, obligatory: 5, showMessage: false });
                        $(component).keydown(function (event) {
                            var value = $(component).val();
                            var code = event.which ? event.which : event.keyCode;
                            if (value.length == 2 && code != 8) {
                                $(component).val(value + ":");
                            }
                            if (!isNull(options.additional) && !isNull(value)) {
                                if (options.additional(event, event.type)) $(component).attr('validated', true);
                                else $(component).attr('validated', false);
                            }
                        });
                        $(component).keyup(function (event) {
                            var value = $(component).val();
                            if (!isNull(value)) {
                                if (value.length == 2) {
                                    if (parseInt(value) > 23) {
                                        $(component).val("");
                                        General.Utils.ShowMessage(TypeMessage.Error, "La hora ingresada es inv\u00E1lida.")
                                    }
                                } else if (value.length == 5) {
                                    var minute = parseInt(value.substring(3, 5));
                                    if (minute > 59) {
                                        $(this).val(value.substring(0, 2) + ":");
                                        General.Utils.ShowMessage(TypeMessage.Error, "El minuto ingresado es inv\u00E1lida.")
                                    }
                                }
                            }
                            if (!isNull(options.additional) && !isNull(value)) {
                                if (options.additional(event, event.type)) $(component).attr('validated', true);
                                else $(component).attr('validated', false);
                            }
                        });
                        $(component).focusout(function (event) {
                            var value = ($(component).val()).toString();
                            if (!isNull(value)) {
                                var hour_in_string = value.substring(0, 2), day = parseInt(value.substring(0, 2));
                                var minute_in_string = value.substring(3, 5), month = parseInt(value.substring(3, 5));
                                if (!(minute_in_string < 59 && hour_in_string <= 23)) {
                                    if (options.showMessage) { General.Utils.ShowMessage(TypeMessage.Error, 'La hora ingresada es inv\u00E1lida.'); }
                                    $(component).val("");
                                    $(component).focus();
                                    return false;
                                }
                            }
                            if (value.length >= 5) { $(component).val(value.substring(0, 5)); }
                            if (value.length == 5) { $(component).closest('.form-group').removeClass('has-error'); }
                            if (!isNull(options.additional) && !isNull(value)) {
                                if (options.additional(event, event.type)) $(component).attr('validated', true);
                                else $(component).attr('validated', false);
                            }
                        });
                        break;

                    /**
                     * @author Fabián Pérez Vásquez
                     * Validación de correo electrónico
                     */
                    case TypeValidation.Email:
                        $(component).off();
                        $(component).keypress(function (event) {
                            var character = String.fromCharCode(event.which);
                            if (Regex.EMail.test(character)) {
                                event.preventDefault();
                            }
                        });
                        $(component).focusout(function (e) {
                            var value = $(component).val();
                            if (!isNull(value)) {
                                if (!Regex.EMail.test(value)) {
                                    if (options.showMessage) { General.Utils.ShowMessage(TypeMessage.Error, 'El email ingresado es inv\u00E1lido.'); }
                                    $(component).focus();
                                }
                            }
                        });
                        break;

                    /**
                     * @author Fabián Pérez Vásquez
                     * Validación de límite de caracteres
                     * @param (opcional) max [default: null] Máxima cantidad de caracteres 
                     * @param (opcional) min [default: null] Mínima cantidad de caracteres
                     * @param (opcional) obligatory [default: null] Cantidad obligatoria de caracteres
                     */
                    case TypeValidation.Limit:
                        if (!isNull(options.max)) $(component).attr('maxlength', options.max);
                        if (!isNull(options.obligatory)) $(component).attr('maxlength', options.obligatory);
                        $(component).focusout(function (event) {
                            var value = ($(component).val()).toString();
                            if (!isNull(value)) {
                                if (!isNull(options.min)) {
                                    if (value.length < options.min) {
                                        event.preventDefault();
                                        if (options.showMessage) { General.Utils.ShowMessage(TypeMessage.Error, 'El campo en el que se encuentra debe tener ' + options.min + ' caracteres como m\u00EDnimo.'); }
                                        $(component).attr('validated', false);
                                        $(component).focus();
                                    }
                                }
                                if (!isNull(options.obligatory)) {
                                    if (value.length != options.obligatory) {
                                        event.preventDefault();
                                        if (options.showMessage) { General.Utils.ShowMessage(TypeMessage.Error, 'El campo en el que se encuentra debe tener ' + options.obligatory + ' caracteres obligatoriamente.'); }
                                        $(component).attr('validated', false);
                                        $(component).focus();
                                    }
                                }
                            }
                        });
                        break;

                    /**
                     * @author Fabián Pérez Vásquez
                     * Validación de números iguales, no se pueden repetir en el campo enviado como parámetro.
                     * @param noRepeat Arreglo, si son varios caracteres; Texto, si es un caracter
                     */
                    case TypeValidation.NoRepeat:
                        $(component).focusout(function (event) {
                            var value = ($(component).val()).toString();
                            if (!isNull(value)) {
                                if (typeof options.noRepeat === 'string') {
                                    var repeat = "";
                                    for (i = 0; i < value.length; i++) repeat += options.noRepeat;
                                    if (equalsIgnoreCase(repeat, value)) {
                                        event.preventDefault();
                                        if (options.showMessage) { General.Utils.ShowMessage(TypeMessage.Error, 'El campo en el que se encuentra no admite la repetici\u00F3n del caracter ' + options.noRepeat + '.'); }
                                        $(component).focus();
                                    }
                                } else {
                                    var repeats = [];
                                    for (i = 0; i < options.noRepeat.length; i++) {
                                        var character = options.noRepeat[i], repeat = '';
                                        for (c = 0; c < value.length; c++) {
                                            repeat += character;
                                        }
                                        repeats.push(repeat);
                                    }
                                    for (i = 0; i < repeats.length; i++) {
                                        var repeat = repeats[i];
                                        if (equalsIgnoreCase(repeat.charAt(0), value.charAt(0))) {
                                            if (equalsIgnoreCase(repeat, value)) {
                                                event.preventDefault();
                                                if (options.showMessage) { General.Utils.ShowMessage(TypeMessage.Error, 'El campo en el que se encuentra no admite la repetici\u00F3n del caracter ' + repeat.charAt(0) + '.'); }
                                                $(component).val("");
                                                $(component).focus();
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                        });
                        break;

                }
            });
        }
    });
})(jQuery);

var General = {
    Init: function () {
        //InitiateSideMenu();
        InitiateWidgets();
    },
    Utils: {

        /**
         * Obtiene la ruta del proyecto en tiempo real durante la compilación
         * @param url Dirección de una página a redireccíonar o un método al que se desea acceder por una petición ajax
         */
        ContextPath: function (url) {
            var finalContext = Context + url;
            return finalContext;
        },

        /**
         * Emite un mensaje corto al lado derecho inferior por defecto
         * @param type [TypeMessage || String] Tipo de mensaje a emitir
         * @param message Mensaje a emitir
         * @param position (opcional) Posición en que desea que salga el mensaje
         */
        ShowMessage: function (type, message, time) {
            switch (type) {
                case TypeMessage.Primary:
                    notify(message, "bottom-right", !isNull(time) ? time : 3000, 'success', '', true);
                    break;
                case TypeMessage.Error:
                    notify(message, "bottom-right", !isNull(time) ? time : 3000, 'error', '', true);
                    break;
                case TypeMessage.Success:
                    notify(message, "bottom-right", !isNull(time) ? time : 3000, 'success', '', true);
                    break;
                case TypeMessage.Information:
                    notify(message, "bottom-right", !isNull(time) ? time : 3000, 'info', '', true);
                    break;
                case TypeMessage.Warning:
                    notify(message, "bottom-right", !isNull(time) ? time : 3000, 'warning', '', true);
                    break;
            }
        },

        /**
         * Emite un mensaje corto al lado derecho inferior por defecto, recibiendo un mensaje de tipo EMensaje
         * @param message Mensaje serializado de la entidad EMensaje, la cual contiene el tipo de mensaje y el contenido del mismo
         * @param position (opcional) Posición en que desea que salga el mensaje
         */
        ShowMessageRequest: function (message, time) {
            if (message["Id"] == TypeMessage.Success) {
                notify(message["Message"], 'bottom-right', !isNull(time) ? time : 3000, 'success', '', true);
            } else if (message["Id"] == TypeMessage.Error) {
                notify(message["Message"], 'bottom-right', !isNull(time) ? time : 3000, 'error', '', true);
            } else if (message["Id"] == TypeMessage.Warning) {
                notify(message["Message"], 'bottom-right', !isNull(time) ? time : 3000, 'warning', '', true);
            } else if (message["Id"] == TypeMessage.Information) {
                notify(message["Message"], 'bottom-right', !isNull(time) ? time : 3000, 'info', '', true);
            }
        },

        /**
         * Emite un mensaje en una ventana emergente pequeña
         * @param type [TypeMessage || String] Tipo de mensaje a emitir
         * @param title Título del mensaje a emitir
         * @param content Contenido del mensaje a emitir
         */
        ShowModalMessage: function (type, title, content, ok, width) {
            content = content.replace(/\n/g, "<br />");
            switch (type) {
                case TypeMessage.Success:
                    $('#modal-success .modal-title').html(title);
                    $('#modal-success .modal-body').html(content);
                    $('#modal-success .modal-dialog').css('width', width);
                    if (!isNull(ok)) {
                        $('#modal-success .btn-success').click(function () {
                            ok();
                        });
                    } else {
                        $('#modal-success .btn-success').click(function () { return true; });
                    }
                    $('#modal-success').modal({ backdrop: 'static', keyboard: false });
                    break;
                case TypeMessage.Information:
                    $('#modal-info .modal-title').html(title);
                    $('#modal-info .modal-body').html(content);
                    $('#modal-info .modal-dialog').css('width', width);
                    if (!isNull(ok)) {
                        $('#modal-info .btn-info').click(function () {
                            ok();
                        });
                    } else {
                        $('#modal-info .btn-info').click(function () { return true; });
                    }
                    $('#modal-info').modal({ backdrop: 'static', keyboard: false });
                    break;
                case TypeMessage.Warning:
                    $('#modal-warning .modal-title').html(title);
                    $('#modal-warning .modal-body').html(content);
                    $('#modal-warning .modal-dialog').css('width', width);
                    if (!isNull(ok)) {
                        $('#modal-warning .btn-warning').click(function () {
                            ok();
                        });
                    } else {
                        $('#modal-warning .btn-warning').click(function () { return true; });
                    }
                    $('#modal-warning').modal({ backdrop: 'static', keyboard: false });
                    break;
                case TypeMessage.Error:
                    $('#modal-error .modal-title').html(title);
                    $('#modal-error .modal-body').html(content);
                    $('#modal-error .modal-dialog').css('width', width);
                    if (!isNull(ok)) {
                        $('#modal-error .btn-danger').click(function () {
                            ok();
                        });
                    } else {
                        $('#modal-error .btn-danger').click(function () { return true; });
                    }
                    $('#modal-error').modal({ backdrop: 'static', keyboard: false });
                    break;
            }
            $('.modal-message').find('.modal-dialog').css('width', '300px;');
        },

        /**
         * Emite una confirmación hacia el usuario, bloqueando toda la pantalla
         * @param width Ancho de la confirmación
         * @param question Pregunta emitida hacia el usuario
         * @param content [HTML] Contenido de la confirmación
         * @param yes [function] Función a realizar cuando el usuario presione el botón (SI)
         * @param no [function] Función a realizar cuando el usuario presione el botón (NO)
         */
        ShowConfirm: function (width, question, content, yes, no, close) {
            $('.btn-cofirm').off();

            $.magnificPopup.open({
                tClose: 'Cerrar',
                items: {
                    src: '<div class="modal-dialog modal-sm">' +
                    '<div class="modal-content">' +
                    '<div class="modal-header">' +
                    '<h4 class="modal-title">Confirmaci&oacute;n</h4>' +
                    '</div>' +
                    '<div class="modal-body">' +
                    '<div class="row">' +
                    '<div class="col-lg-12 text-center">' +
                    '<div class="form-group">' +
                    '<b style="color: black;">' + question + '</b>' +
                    (isNull(content) ? '' : '<p>' + content + '</p>') +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '<div class="row">' +
                    '<div class="col-lg-12 text-center">' +
                    '<button id="oft-confirm-yes" class="btn btn-xs btn-primary btn-confirm">S&iacute;</button>' +
                    '&nbsp;' +
                    '<button id="oft-confirm-no" class="btn btn-xs btn-confirm">No</button>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>' +
                    '</div>',
                    type: 'inline',
                },
                closeOnBgClick: false,
                enableEscapeKey: false,
                showCloseBtn: false
            });

            document.getElementById('oft-confirm-yes').addEventListener('click', yes);
            document.getElementById('oft-confirm-no').addEventListener('click', no);

            if (isNull(close)) {
                $('.btn-confirm').click(function () {
                    $.magnificPopup.close();
                });
            }
        },

        /**
         * Cierra todos los mensajes cortos emitidos al usuario
         */
        ClearShortMessages: function () {
            toastr.clear();
        },

        /**
         * Redirecciona hacia una página específica
         * @param Controlador
         * @param ActionResult (con o sin parámetros)
         */
        Redirect: function (controller, action) {
            location.href = General.Utils.ContextPath(controller + "/" + action);
        },

        CompareUrl: function (link) {
            var CurrentLocation = window.location.href.split("?")[0];
            if (equalsIgnoreCase(CurrentLocation, General.Utils.ContextPath(link))) {
                return true;
            }
            return false;
        },

        /**
         * Verificación del formulario de acuerdo a las propiedades de la extensión Validate
         * @param form Formulario a obtener su validación
         */
        ValidateForm: function (form) {
            var $form = $(form);

            // Verificación de valores nulos
            var areThereNulls = false;
            $form.find('.no-null').each(function (index, item) {
                var $component = $(this), value = $component.val();
                if (isNull(value)) {
                    $component.closest('div.form-group').addClass('has-error');
                    areThereNulls = true;
                } else {
                    $component.closest('div.form-group').removeClass('has-error');
                }
            });
            if (areThereNulls) { General.Utils.ShowMessage(TypeMessage.Error, 'Debe llenar los campos obligatorios.'); return false; }

            // Verificación de componentes no validades
            var areNoValidateds = false;
            $form.find('.form-control').each(function (index, item) {
                var $component = $(this), isValidated = $component.attr("validated");
                if (!isNull(isValidated)) {
                    if (equalsIgnoreCase(isValidated, 'false')) {
                        $component.closest('div.form-group').addClass('has-error');
                        areNoValidateds = true;
                    }
                }
            });
            if (areNoValidateds) { General.Utils.ShowMessage(TypeMessage.Error, 'Verifique los campos se\u00F1lizados, los cuales son incorrectos.'); return false; }

            return true;
        },

        /**
         * Obtiene los datos de los formularios en un objeto JSON
         * @param form Formulario a serializar
         */
        SerializeForm: function (form) {
            var $form = $(form);
            var serialization = '{';
            $form.find('.form-control').each(function (index, item) {
                var value = $(this).val(), name = $(this).attr('name'), component = $(this);
                serialization +=
                    '"' + name + '": {' +
                    '"value" : ' + '"' + value + '"' + ', ' +
                    '"name" : "' + name + '", ' +
                    '"component" : ' + '".form-control[name=\'' + name + '\']"' +
                    '},';
            });
            serialization = serialization.substring(0, serialization.length - 1);
            serialization += '}';
            return $.parseJSON(serialization);
        },

        DateAdd: function (type, date, add) {

            if (isNull(type)) {
                console.error('General: No se ha especificado un tipo de adici\u00F3n.')
                return null;
            }

            var milli_seconds = parseInt(35 * 24 * 60 * 60 * 1000);
            switch (type) {
                case 'day':
                    day = date.getDate();
                    month = date.getMonth() + 1;
                    year = date.getFullYear();

                    time = date.getTime();
                    milli_seconds = parseInt(add * 24 * 60 * 60 * 1000);
                    total = date.setTime(time + milli_seconds);
                    day = date.getDate();
                    month = date.getMonth() + 1;
                    year = date.getFullYear();

                    return new Date(year, month - 1, day);
                case 'month':
                    return new Date();
                case 'year':
                    return new Date();
            }
        },

        StartLoading: function () {
            $.magnificPopup.open({
                tClose: 'Cerrar',
                items: {
                    src: '<div style="position: relative; margin: 20px auto; text-align: center; z-index: 1051 !important;" >' +
                    '<p style="color: #FFF; font-size: 20px;">Cargando...</p>' +
                    '</div>',
                    type: 'inline'
                },
                closeOnBgClick: false,
                enableEscapeKey: false,
                showCloseBtn: false
            });
        },

        EndLoading: function () {
            $.magnificPopup.close();
        },

        Month: {

            /**
             * Obtiene el valor relacionado con el mes enviado, según el tipo de valor que se requiera
             * @param value Valor a convertir
             * @param input Tipo de valor entrante
             * @param output Tipo de valor saliente
             */
            Get: function (value, input, output) {
                var month = "";
                $.grep(Months, function (item) {
                    if (value == item[input]) {
                        month = item[output];
                    }
                });
                return month;
            },
            ShortName: 'short_name',
            Name: 'name',
            Number: 'number'
        }
    },
    Convert: {

        /**
         * @author Fabián Pérez Vásquez
         * Convierte el texto entregado como parámetro en una palabra con
         * la primera letra en mayúscula y las demás en minúsculas, verificando
         * los espacios que hay en dicho texto
         * @param value Texto a convertir
         * @return String
         */
        ToFirstCapital: function (value) {
            var output = "";
            var words = value.split(" ");

            for (w = 0; w < words.length; w++) {
                for (i = 0; i < words[w].length; i++) {
                    var character = words[w].charAt(i);
                    switch (i) {
                        case 0: {
                            if (words[w].length == 1) {
                                output += character.toLowerCase();
                                output += " "
                            } else {
                                output += character.toUpperCase();
                            }
                            if (!Regex.Letter.exec(character)) {
                                i++;
                                output += words[w].charAt(i).toUpperCase();
                                if (i == words[w].length - 1) {
                                    output += ' ';
                                }
                            }
                        } break;
                        case words[w].length - 1: {
                            output += character.toLowerCase();
                            output += " "
                        } break;
                        default: {
                            output += character.toLowerCase();
                            if (!Regex.Letter.exec(character)) {
                                i++;
                                output += words[w].charAt(i).toUpperCase();
                                if (i == words[w].length - 1) {
                                    output += ' ';
                                }
                            }
                        } break;
                    }
                }
            }

            return output;
        },

        /**
         * @author Fabián Pérez Vásquez
         * Convierte el texto entregado a mayúsculas
         * @param value Texto a convertir
         * @return String
         */
        ToUpper: function (value) {
            if (isNull(value))
                return '';
            return value.toString().toUpperCase();
        },

        /**
         * @author Fabián Pérez Vásquez
         * Convierte el texto entregado a mayúsculas
         * @param date Fecha a convertir
         * @return String
         */
        FromDateToText: function (date) {
            return (date.getDate() < 10 ? '0' + date.getDate() : date.getDate()) + '/' +
                ((date.getMonth() + 1) < 10 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/' + date.getFullYear();
        },

        FromDateToTextEn: function (date) {
            return date.getFullYear() + '-' + ((date.getMonth() + 1) < 10 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '-' + (date.getDate() < 10 ? '0' + date.getDate() : date.getDate());
        },

        FromDateToCompleteText: function (date) {
            return (date.getDate() < 10 ? '0' + date.getDate() : date.getDate()) + '/' +
                ((date.getMonth() + 1) < 10 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)) + '/' + date.getFullYear() +
                ' ' + (date.getHours() < 10 ? '0' + date.getHours() : date.getHours()) + ':' +
                (date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes()) + ':' +
                (date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds());
        },

        FromServerDateFromDate: function (date) {
            return new Date(date.match(/\d+/)[0] * 1);
        },

        /**
         * @author Fabián Pérez Vásquez
         * Convierte el texto entregado a mayúsculas
         * @param type Tipo de formato de fecha que está recbiendo
         * @param text Texto a convertir
         * @return Date
         */
        FromTextToDate: function (type, text) {
            var date;
            switch (type) {
                case 'short':
                    var text_split = text.split('/');
                    var day = parseInt(text_split[0]);
                    var month = parseInt(text_split[1]) - 1;
                    var year = parseInt(text_split[2]);
                    date = new Date(year, month, day);
                    break;

                case 'full':
                    var date_split = text.substring(0, 10).split('/');
                    var day = parseInt(date_split[0]);
                    var month = parseInt(date_split[1]) - 1;
                    var year = parseInt(date_split[2]);
                    var hour_split = text.substring(11, 16).split(':');
                    var hour = parseInt(hour_split[0]);
                    var minute = parseInt(hour_split[1]) - 1;
                    date = new Date(year, month, day, hour, minute);
                    break;
            }
            return date;
        },

        NumberWithSpace: function (value) {
            return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, " ");
        },

        DateFormat: function (text) {
            var date;
            var day = text.substring(0, 2);
            var month = text.substring(3, 5);
            var year = text.substring(6, 10);
            return year + '-' + month + '-' + day;
        }
    },
    Get: {
        UrlParameter: function (param) {
            var url = decodeURIComponent(window.location.search.substring(1)),
                vars = url.split('&'),
                name,
                i;
            for (i = 0; i < vars.length; i++) {
                name = vars[i].split('=');
                if (name[0] === param) {
                    return name[1] === undefined ? true : name[1];
                }
            }
        }
    }
}

/**************************************************************
 **************************************************************
 FUNCIONES DE ACCIÓN
 **************************************************************
  *************************************************************/

function initClock(datetime) {
    var sDia = datetime.split(" ")[0],
        sHora = datetime.split(" ")[1];
    var iAnio = sDia.split("/")[2],
        iMes = sDia.split("/")[1],
        iDia = sDia.split("/")[0];
    var iHora = parseInt(sHora.split(":")[0]),
        iMinuto = parseInt(sHora.split(":")[1]),
        iSegundo = parseInt(sHora.split(":")[2]);
    var dFecha = new Date(iAnio, iMes - 1, iDia, iHora, iMinuto, iSegundo);
    var dFechaLocal = new Date();
    var iDiff = dFecha - dFechaLocal;

    setInterval(function () {
        var dFechaFinal = new Date(Date.now() + iDiff);
        $('#spHora').html((dFechaFinal.getUTCDate() < 10 ? '0' + dFechaFinal.getUTCDate() : dFechaFinal.getUTCDate()) + '/' +
            ((dFechaFinal.getMonth() + 1) < 10 ? '0' + (dFechaFinal.getMonth() + 1) : (dFechaFinal.getMonth() + 1)) + '/' +
            dFechaFinal.getFullYear() + ' ' +
            (dFechaFinal.getHours() < 10 ? '0' + dFechaFinal.getHours() : dFechaFinal.getHours()) + ':' +
            (dFechaFinal.getMinutes() < 10 ? '0' + dFechaFinal.getMinutes() : dFechaFinal.getMinutes()) + ':' +
            (dFechaFinal.getSeconds() < 10 ? '0' + dFechaFinal.getSeconds() : dFechaFinal.getSeconds()));
    }, 1000)
}

function addTime(start, end) {
    var iHoraInicio = parseInt(start.split(":")[0]),
        iMinutoInicio = parseInt(start.split(":")[1]);
    var iHoraFin = parseInt(end.split(":")[0]) == 0 ? iHoraInicio : parseInt(end.split(":")[0]),
        iMinutoFin = parseInt(end.split(":")[1]);
    var iHoraFinal, iMinutoFinal;
    if (iHoraInicio == iHoraFin) {
        iHoraFinal = iHoraInicio;
    } else {
        iHoraFinal = iHoraInicio + iHoraFin;
    }
    iMinutoFinal = iMinutoInicio + iMinutoFin;
    if (iMinutoFinal > 60) {
        iHoraFinal++;
        iMinutoFinal -= 60;
    }
    var dAdd = new Date(2000, 0, 1, iHoraFinal, iMinutoFinal);
    return (dAdd.getHours() < 10 ? '0' + dAdd.getHours() : dAdd.getHours()) + ':' + (dAdd.getMinutes() < 10 ? '0' + dAdd.getMinutes() : dAdd.getMinutes());
}

/**************************************************************
 **************************************************************
 FUNCIONES DE VERIFICACIÓN
 **************************************************************
  *************************************************************/

// Verifica si el valor entregado como parámetro es nulo, validando si este presenta comillas vacías, el valor -1 o si
/**
 * @author Fabián Pérez Vásquez
 * es nulo.
 * @param value - Valor a verificar
 * @return Boolean
 */
function isNull(value) {
    if (typeof value === 'undefined') {
        return true;
    } else if (value == null) {
        return true;
    } else if (value == "") {
        return true;
    } else if (value == -1) {
        return true;
    } else {
        return false;
    }
}

function into(comparer, values) {
    var isInArray = false;
    $.grep(values, function (item) {
        if (item == comparer) {
            isInArray = true;
        }
    })
    return isInArray;
}

/**
 * @author Fabián Pérez Vásquez
 * Compara dos textos o números entregados como parámetros,
 * verificando si estos dos son iguales o diferentes
 * @param value - Primer valor a comparar
 * @param comparer - Segundo valor a comparar
 * @return Boolean
 */
function equalsIgnoreCase(value, comparer) {
    if (General.Convert.ToUpper(value).trim() == General.Convert.ToUpper(comparer).trim()) {
        return true;
    }
    return false;
}

/**
 * @author Fabián Pérez Vásquez
 * Verificación se la fecha enviada como parámetro es menor a la fecha de hoy
 * @param date Fecha a validar
 * @return Boolean
 */
function isBeforeToday(date) {
    if (typeof date == 'string') {
        var iDia = parseInt(date.substring(0, 2));
        var iMes = parseInt(date.substring(3, 5));
        var iAnio = parseInt(date.substring(6, 10));
        if (new Date(iAnio, iMes - 1, iDia).setHours(0, 0, 0, 0) >= new Date().setHours(0, 0, 0, 0)) {
            return false;
        }
        return true;
    } else {
        if (date >= new Date()) {
            return false;
        }
        return true;
    }
}

function isToday(date) {
    if (typeof date == 'string') {
        var sDia = date.substring(0, 2);
        var sMes = date.substring(3, 5);
        var sAnio = date.substring(6, 10);
        if (new Date(sMes + '/' + sDia + '/' + sAnio).setHours(0, 0, 0, 0) == new Date().setHours(0, 0, 0, 0)) {
            return true;
        }
        return false;
    } else {
        if (date.setHours(0, 0, 0, 0) == new Date().setHours(0, 0, 0, 0)) {
            return true;
        }
        return false;
    }
}

function isYesterday(date) {
    var dateIn = new Date(parseInt(date.split("/")[2]), parseInt(date.split("|")[1]) - 1, parseInt(date.split("|")[0]));
    var todayTimeStamp = +new Date; // Unix timestamp in milliseconds
    var oneDayTimeStamp = 1000 * 60 * 60 * 24; // Milliseconds in a day
    var diff = todayTimeStamp - oneDayTimeStamp;
    var yesterdayDate = new Date(diff);
    if (dateIn.getFullYear() == yesterdayDate.getFullYear()) {
        if (dateIn.getMonth() == yesterdayDate.getMonth()) {
            if (dateIn.getDate() - 1 == yesterdayDate.getDate()) {
                return true;
            }
        }
    }
    return false;
}

function isTomorrow(date) {
    var dateIn = new Date(parseInt(date.split("/")[2]), parseInt(date.split("/")[1]) - 1, parseInt(date.split("/")[0]));
    var todayTimeStamp = +new Date; // Unix timestamp in milliseconds
    var oneDayTimeStamp = 1000 * 60 * 60 * 24; // Milliseconds in a day
    var diff = todayTimeStamp + oneDayTimeStamp;
    var tomorrowDate = new Date(diff);
    if (dateIn.getFullYear() == tomorrowDate.getFullYear()) {
        if (dateIn.getMonth() == tomorrowDate.getMonth()) {
            if (dateIn.getDate() == tomorrowDate.getDate()) {
                return true;
            }
        }
    }
    return false;
}

function isTimeBeforeThan(start, end) {
    var iStartHour = parseInt(start.split(":")[0]);
    var iStartMinute = parseInt(start.split(":")[1]);
    var iEndHour = parseInt(end.split(":")[0]);
    var iEndMinute = parseInt(end.split(":")[1]);
    if (new Date(1, 0, 2000, iStartHour, iStartMinute) < new Date(1, 0, 2000, iEndHour, iEndMinute)) {
        return true;
    }
    return false;
}

function isDateBeforeThan(start, end) {
    var iStartDay = parseInt(start.split("/")[0]);
    var iStartMonth = parseInt(start.split("/")[1]);
    var iStartYear = parseInt(start.split("/")[2]);
    var iEndDay = parseInt(end.split("/")[0]);
    var iEndMonth = parseInt(end.split("/")[1]);
    var iEndYear = parseInt(end.split("/")[2]);
    if (new Date(iStartYear, iStartMonth + 1, iStartDay) <= new Date(iEndYear, iEndMonth + 1, iEndDay)) {
        return true;
    }
    return false;
}

function startsWith(value, character) {
    if (!isNull(value)) {
        if (value.charAt(0) == character) {
            return true;
        }
    }
}

function isNotSymbol(character) {
    if (character != 8 && character != 9 && character != 16 && character != 36 && character != 15
        && character != 37 && character != 38 && character != 39) {
        return true;
    }
    return false;
}

function notify(message, position, timeout, theme, icon, closable) {
    toastr.options.positionClass = 'toast-' + position;
    toastr.options.extendedTimeOut = 0; //1000;
    toastr.options.timeOut = timeout;
    toastr.options.closeButton = closable;
    toastr.options.iconClass = icon + ' toast-' + theme;
    toastr['custom'](message);
}

function toFixed(num, precision) {
    return (+(Math.round(+(num + 'e' + precision)) + 'e' + -precision)).toFixed(precision);
}

function customJSONstringify(obj) {
    return JSON.stringify(obj).replace(/\/Date/g, "\\\/Date").replace(/\)\//g, "\)\\\/")
}

$.datepicker.regional['es'] = {
    closeText: 'Cerrar',
    prevText: '<Ant',
    nextText: 'Sig>',
    currentText: 'Hoy',
    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sabado'],
    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Juv', 'Vie', 'Sab'],
    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
    weekHeader: 'Sm',
    dateFormat: 'dd/mm/yy',
    firstDay: 1,
    isRTL: false,
    showMonthAfterYear: false,
    yearSuffix: ''
};

String.prototype.replaceAll = function (stringFind, stringReplace) {
    var ex = new RegExp(stringFind.replace(/([.*+?^=!:${}()|\[\]\/\\])/g, "\\$1"), "g");
    return this.replace(ex, stringReplace);
};

function padding_right(s, c, n) {
    if (!s || !c || s.length >= n) {
        return s;
    }
    var max = (n - s.length) / c.length;
    for (var i = 0; i < max; i++) {
        s += c;
    }
    return s;
}

function padding_left(s, c, n) {
    if (!s || !c || s.length >= n) {
        return s;
    }
    var max = (n - s.length) / c.length;
    for (var i = 0; i < max; i++) {
        s = c + s;
    }
    return s;
}

function format_two_digits(n) {
    return n < 10 ? '0' + n : n;
}

function time_format(d) {
    hours = format_two_digits(d.getHours());
    minutes = format_two_digits(d.getMinutes());
    seconds = format_two_digits(d.getSeconds());
    return hours + ":" + minutes + ":" + seconds;
}
/*
function startConnection(config) {
    if (!qz.websocket.isActive()) {
        try{
        qz.websocket.connect(config).then(function ) {
            findPrinter('zebra', true);
        }).catch(handleConnectionError);
    } else {
        displayMessage('An active connection with QZ already exists.', 'alert-warning');
    }
}
*/
function handleConnectionError(err) {
    console.error(err);
}

var cfg = null;
function getUpdatedConfig() {
    if (cfg == null) {
        cfg = qz.configs.create("zebra");
    }
    return cfg
}

function displayError(err) {
    console.error(err);
}

function findPrinter(query, set) {
    qz.printers.find(query).then(function (data) {
        if (set) { setPrinter(data); }
    }).catch(displayError);
}

function setPrinter(printer) {
    var cf = getUpdatedConfig();
    cf.setPrinter(printer);

    if (typeof printer === 'object' && printer.name == undefined) {
        var shown;
        if (printer.file != undefined) {
            shown = "<em>FILE:</em> " + printer.file;
        }
        if (printer.host != undefined) {
            shown = "<em>HOST:</em> " + printer.host + ":" + printer.port;
        }

    } else {
        if (printer.name != undefined) {
            printer = printer.name;
        }

        if (printer == undefined) {
            printer = 'NONE';
        }
    }
}

function formatNumber(num) {
    if (!num || num == 'NaN') return '0.00';
    if (num == 'Infinity') return '&#x221e;';
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' + num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}