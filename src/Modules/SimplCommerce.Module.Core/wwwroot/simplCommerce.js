var simplCommerce = simplCommerce || {};
(function ($) {


    /* Application paths *****************************************/

    //Current application root path (including virtual directory if exists).
    simplCommerce.appPath = simplCommerce.appPath || '/';
    simplCommerce.pageLoadTime = new Date();

    //Converts given path to absolute path using simplCommerce.appPath variable.
    simplCommerce.toAbsAppPath = function (path) {
        if (path.indexOf('/') == 0) {
            path = path.substring(1);
        }

        return simplCommerce.appPath + path;
    };


    /* REALTIME NOTIFICATIONS ************************************/

    simplCommerce.notifications = simplCommerce.notifications || {};

    simplCommerce.notifications.severity = {
        INFO: 0,
        SUCCESS: 1,
        WARN: 2,
        ERROR: 3,
        FATAL: 4
    };

    simplCommerce.notifications.userNotificationState = {
        UNREAD: 0,
        READ: 1
    };

    simplCommerce.notifications.getUserNotificationStateAsString = function (userNotificationState) {
        switch (userNotificationState) {
            case simplCommerce.notifications.userNotificationState.READ:
                return 'READ';
            case simplCommerce.notifications.userNotificationState.UNREAD:
                return 'UNREAD';
            default:
                simplCommerce.log.warn('Unknown user notification state value: ' + userNotificationState)
                return '?';
        }
    };

    simplCommerce.notifications.getUiNotifyFuncBySeverity = function (severity) {
        switch (severity) {
            case simplCommerce.notifications.severity.SUCCESS:
                return simplCommerce.notify.success;
            case simplCommerce.notifications.severity.WARN:
                return simplCommerce.notify.warn;
            case simplCommerce.notifications.severity.ERROR:
                return simplCommerce.notify.error;
            case simplCommerce.notifications.severity.FATAL:
                return simplCommerce.notify.error;
            case simplCommerce.notifications.severity.INFO:
            default:
                return simplCommerce.notify.info;
        }
    };

    simplCommerce.notifications.messageFormatters = {};

    simplCommerce.notifications.messageFormatters['simplCommerce.Notifications.MessageNotificationData'] = function (userNotification) {
        return userNotification.notification.data.message || userNotification.notification.data.properties.Message;
    };

    simplCommerce.notifications.messageFormatters['simplCommerce.Notifications.LocalizableMessageNotificationData'] = function (userNotification) {
        var message = userNotification.notification.data.message || userNotification.notification.data.properties.Message;
        var localizedMessage = simplCommerce.localization.localize(
            message.name,
            message.sourceName
        );

        if (userNotification.notification.data.properties) {
            if ($) {
                //Prefer to use jQuery if possible
                $.each(userNotification.notification.data.properties, function (key, value) {
                    localizedMessage = localizedMessage.replace('{' + key + '}', value);
                });
            } else {
                //alternative for $.each
                var properties = Object.keys(userNotification.notification.data.properties);
                for (var i = 0; i < properties.length; i++) {
                    localizedMessage = localizedMessage.replace('{' + properties[i] + '}', userNotification.notification.data.properties[properties[i]]);
                }
            }
        }

        return localizedMessage;
    };

    simplCommerce.notifications.getFormattedMessageFromUserNotification = function (userNotification) {
        var formatter = simplCommerce.notifications.messageFormatters[userNotification.notification.data.type];
        if (!formatter) {
            simplCommerce.log.warn('No message formatter defined for given data type: ' + userNotification.notification.data.type)
            return '?';
        }

        if (!simplCommerce.utils.isFunction(formatter)) {
            simplCommerce.log.warn('Message formatter should be a function! It is invalid for data type: ' + userNotification.notification.data.type)
            return '?';
        }

        return formatter(userNotification);
    }

    simplCommerce.notifications.showUiNotifyForUserNotification = function (userNotification, options) {
        var message = simplCommerce.notifications.getFormattedMessageFromUserNotification(userNotification);
        var uiNotifyFunc = simplCommerce.notifications.getUiNotifyFuncBySeverity(userNotification.notification.severity);
        uiNotifyFunc(message, undefined, options);
    }

    /* LOGGING ***************************************************/
    //Implements Logging API that provides secure & controlled usage of console.log

    simplCommerce.log = simplCommerce.log || {};

    simplCommerce.log.levels = {
        DEBUG: 1,
        INFO: 2,
        WARN: 3,
        ERROR: 4,
        FATAL: 5
    };

    simplCommerce.log.level = simplCommerce.log.levels.DEBUG;

    simplCommerce.log.log = function (logObject, logLevel) {
        if (!window.console || !window.console.log) {
            return;
        }

        if (logLevel != undefined && logLevel < simplCommerce.log.level) {
            return;
        }

        console.log(logObject);
    };

    simplCommerce.log.debug = function (logObject) {
        simplCommerce.log.log("DEBUG: ", simplCommerce.log.levels.DEBUG);
        simplCommerce.log.log(logObject, simplCommerce.log.levels.DEBUG);
    };

    simplCommerce.log.info = function (logObject) {
        simplCommerce.log.log("INFO: ", simplCommerce.log.levels.INFO);
        simplCommerce.log.log(logObject, simplCommerce.log.levels.INFO);
    };

    simplCommerce.log.warn = function (logObject) {
        simplCommerce.log.log("WARN: ", simplCommerce.log.levels.WARN);
        simplCommerce.log.log(logObject, simplCommerce.log.levels.WARN);
    };

    simplCommerce.log.error = function (logObject) {
        simplCommerce.log.log("ERROR: ", simplCommerce.log.levels.ERROR);
        simplCommerce.log.log(logObject, simplCommerce.log.levels.ERROR);
    };

    simplCommerce.log.fatal = function (logObject) {
        simplCommerce.log.log("FATAL: ", simplCommerce.log.levels.FATAL);
        simplCommerce.log.log(logObject, simplCommerce.log.levels.FATAL);
    };

    /* NOTIFICATION *********************************************/
    //Defines Notification API, not implements it

    simplCommerce.notify = simplCommerce.notify || {};

    simplCommerce.notify.success = function (message, title, options) {
        simplCommerce.log.warn('simplCommerce.notify.success is not implemented!');
    };

    simplCommerce.notify.info = function (message, title, options) {
        simplCommerce.log.warn('simplCommerce.notify.info is not implemented!');
    };

    simplCommerce.notify.warn = function (message, title, options) {
        simplCommerce.log.warn('simplCommerce.notify.warn is not implemented!');
    };

    simplCommerce.notify.error = function (message, title, options) {
        simplCommerce.log.warn('simplCommerce.notify.error is not implemented!');
    };

    /* MESSAGE **************************************************/
    //Defines Message API, not implements it

    simplCommerce.message = simplCommerce.message || {};

    var showMessage = function (message, title) {
        alert((title || '') + ' ' + message);

        if (!$) {
            simplCommerce.log.warn('simplCommerce.message can not return promise since jQuery is not defined!');
            return null;
        }

        return $.Deferred(function ($dfd) {
            $dfd.resolve();
        });
    };

    simplCommerce.message.info = function (message, title) {
        simplCommerce.log.warn('simplCommerce.message.info is not implemented!');
        return showMessage(message, title);
    };

    simplCommerce.message.success = function (message, title) {
        simplCommerce.log.warn('simplCommerce.message.success is not implemented!');
        return showMessage(message, title);
    };

    simplCommerce.message.warn = function (message, title) {
        simplCommerce.log.warn('simplCommerce.message.warn is not implemented!');
        return showMessage(message, title);
    };

    simplCommerce.message.error = function (message, title) {
        simplCommerce.log.warn('simplCommerce.message.error is not implemented!');
        return showMessage(message, title);
    };

    simplCommerce.message.confirm = function (message, titleOrCallback, callback) {
        simplCommerce.log.warn('simplCommerce.message.confirm is not implemented!');

        if (titleOrCallback && !(typeof titleOrCallback == 'string')) {
            callback = titleOrCallback;
        }

        var result = confirm(message);
        callback && callback(result);

        if (!$) {
            simplCommerce.log.warn('simplCommerce.message can not return promise since jQuery is not defined!');
            return null;
        }

        return $.Deferred(function ($dfd) {
            $dfd.resolve();
        });
    };

    /* UI *******************************************************/

    simplCommerce.ui = simplCommerce.ui || {};

    /* UI BLOCK */
    //Defines UI Block API, not implements it

    simplCommerce.ui.block = function (elm) {
        simplCommerce.log.warn('simplCommerce.ui.block is not implemented!');
    };

    simplCommerce.ui.unblock = function (elm) {
        simplCommerce.log.warn('simplCommerce.ui.unblock is not implemented!');
    };

    /* UI BUSY */
    //Defines UI Busy API, not implements it

    simplCommerce.ui.setBusy = function (elm, optionsOrPromise) {
        simplCommerce.log.warn('simplCommerce.ui.setBusy is not implemented!');
    };

    simplCommerce.ui.clearBusy = function (elm) {
        simplCommerce.log.warn('simplCommerce.ui.clearBusy is not implemented!');
    };

    /* SIMPLE EVENT BUS *****************************************/

    simplCommerce.event = (function () {

        var _callbacks = {};

        var on = function (eventName, callback) {
            if (!_callbacks[eventName]) {
                _callbacks[eventName] = [];
            }

            _callbacks[eventName].push(callback);
        };

        var off = function (eventName, callback) {
            var callbacks = _callbacks[eventName];
            if (!callbacks) {
                return;
            }

            var index = -1;
            for (var i = 0; i < callbacks.length; i++) {
                if (callbacks[i] === callback) {
                    index = i;
                    break;
                }
            }

            if (index < 0) {
                return;
            }

            _callbacks[eventName].splice(index, 1);
        };

        var trigger = function (eventName) {
            var callbacks = _callbacks[eventName];
            if (!callbacks || !callbacks.length) {
                return;
            }

            var args = Array.prototype.slice.call(arguments, 1);
            for (var i = 0; i < callbacks.length; i++) {
                callbacks[i].apply(this, args);
            }
        };

        // Public interface ///////////////////////////////////////////////////

        return {
            on: on,
            off: off,
            trigger: trigger
        };
    })();






})(jQuery);
