// Wait for Cordova to load
document.addEventListener('deviceready', onDeviceReady, false);

// Cordova is ready
function onDeviceReady() {
    var db = openDatabase('SGQUSA', '1.0', 'WebSQL Database', 2 * 1024 * 1024);

    //PeriodHTMLDAO.dropTable();
    PeriodHTMLDAO.createTable();
    PeriodHTMLDAO.insertHTML();
    PeriodHTMLDAO.updateHTML(1);
    PeriodHTMLDAO.selectTable(list);

}

var SystemDAO = {
    getDB: function () {
        return openDatabase('SGQUSA', '1.0', 'WebSQL Database', 2 * 1024 * 1024);
    }
}

var PeriodHTMLDAO = {
    createTable: function () {
        SystemDAO.getDB().transaction(function (tx) {
            tx.executeSql("CREATE TABLE IF NOT EXISTS PeriodHTML (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ValueHtml TEXT, InsertDate TEXT, SyncDate TEXT, Shift INTEGER, Period INTEGER)");
        });
    },

    dropTable: function () {
        SystemDAO.getDB().transaction(function (tx) {
            tx.executeSql('DROP TABLE IF EXISTS PeriodHTML');
        });
    },

    insertHTML: function () {
        SystemDAO.getDB().transaction(function (tx) {
            var date = new Date();
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            var day = date.getDate();
            var hour = date.getHours();
            var minute = date.getMinutes();
            var second = date.getSeconds();
            var yyyymmddhhmm = year + ("0" + month).slice(-2) + "" + ("0" + day).slice(-2) + "" + ("0" + hour).slice(-2) + "" + ("0" + minute).slice(-2) + "" + ("0" + second).slice(-2);
            var dt = year + "/" + ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + " " + ("0" + hour).slice(-2) + ":" + ("0" + minute).slice(-2) + ":" + ("0" + second).slice(-2);

            var shift = $('.App').attr('shift');
            //var period = $('.App').attr('period');
            var html = $('.App').html();

            tx.executeSql("INSERT INTO PeriodHTML (ValueHtml, InsertDate, Shift, Period) VALUES ('" + html + "', '" + dt + "', '1', '1') ", null, null, null);
        });
    },

    updateHTML: function (id) {
        SystemDAO.getDB().transaction(function (tx) {
            var date = new Date();
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            var day = date.getDate();
            var hour = date.getHours();
            var minute = date.getMinutes();
            var second = date.getSeconds();
            var yyyymmddhhmm = year + ("0" + month).slice(-2) + "" + ("0" + day).slice(-2) + "" + ("0" + hour).slice(-2) + "" + ("0" + minute).slice(-2) + "" + ("0" + second).slice(-2);
            var dt = year + "/" + ("0" + month).slice(-2) + "/" + ("0" + day).slice(-2) + " " + ("0" + hour).slice(-2) + ":" + ("0" + minute).slice(-2) + ":" + ("0" + second).slice(-2);

            var shift = $('.App').attr('shift');
            var period = $('.App').attr('period');
            var contentHtml = $('.App').html();

            tx.executeSql('UPDATE PeriodHTML SET ValueHtml = ?, InsertDate = ?, Shift = ?, Period = ? WHERE Id = ? ', [contentHtml, dt, 1, 1, 1], onSuccess, onError);

        });
    },

    insertTable: function (value) {
        SystemDAO.getDB().transaction(function (tx) {
            var date = new Date();
            var year = date.getFullYear();
            var month = date.getMonth() + 1;
            var day = date.getDate();
            var hour = date.getHours();
            var minute = date.getMinutes();
            var second = date.getSeconds();
            var yyyymmddhhmm = year + ("0" + month).slice(-2) + "" + ("0" + day).slice(-2) + "" + ("0" + hour).slice(-2) + "" + ("0" + minute).slice(-2) + "" + ("0" + second).slice(-2);

            tx.executeSql("INSERT INTO PeriodHTML (Value, InsertDate) VALUES ('" + value + "', '" + yyyymmddhhmm + "') ", null, null, null);
        });
    },

    selectTable: function (callback) {
        return SystemDAO.getDB().transaction(function (tx) {
            tx.executeSql('select * from PeriodHTML', [],
            function (tx, results) {
                return callback(results.rows, results.rows.length - 1);
            });
        });

    },

    appendHTML: function () {
        return SystemDAO.getDB().transaction(function (tx) {
            tx.executeSql('select * from PeriodHTML', [],
            function (tx, results) {
                if (results.rows[results.rows.length - 1] != undefined) {
                    $('.App').empty();
                    $('.App').append(results.rows[results.rows.length - 1].ValueHtml);
                }
            });
        });
    },

    showHTML: function (id) {
        return SystemDAO.getDB().transaction(function (tx) {
            tx.executeSql('select * from PeriodHTML', [],
            function (tx, results) {
                if (results.rows[id].ValueHtml != undefined) {
                    console.log(results.rows[id].ValueHtml);
                }
            });
        });
    }
};

function list(value, last) {
    //$('.App').empty();
    if (value[last].ValueHtml != undefined) {
        $('.App').html(value[last].ValueHtml);
    }
    // $('.App').append(value[last].ValueHtml);
    showLevel01();
    console.log(value); 
}

function onSuccess(transaction, resultSet) {
    console.log('Query completed: ' + JSON.stringify(resultSet));
}

function onError(transaction, error) {
    console.log('Query failed: ' + error.message);
}

//onDeviceReady();