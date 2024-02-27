const liffId = "my-liff-id";
liff.init({ liffId });

// liff.id equals to liffId

liff.ready.then(() => {
  // do something you want when liff.init finishes
});
// Using a Promise object
liff
  .init({
    liffId: "123456-abcedfg", // Use own liffId
  })
  .then(() => {
    // Start to use liff's api
  })
  .catch((err) => {
    // Error happens during initialization
    console.log(err.code, err.message);
  });

// Using a callback
liff.init({ liffId: "123456-abcedfg" }, successCallback, errorCallback);
