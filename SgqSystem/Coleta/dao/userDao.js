//create table user
databaseDAO.transaction(function(transaction) {
    transaction.executeSql('CREATE TABLE IF NOT EXISTS phonegap_pro (id integer primary key, title text, desc text)', [],
        function(tx, result) {
            alert("Table created successfully");
        },
        function(error) {
            alert("Error occurred while creating the table.");
        }
    );
});

//insert user

//read user

//update user

//delete user

//drop table
