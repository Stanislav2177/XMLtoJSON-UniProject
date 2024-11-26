function showSignIn() {
  const loginDiv = document.getElementById("loginFormDiv");
  const registerDiv = document.getElementById("registerFormDiv");

  // Hide the register form and show the login form
  registerDiv.classList.add("hidden");
  loginDiv.classList.remove("hidden");
}

function showSignOn() {
  const loginDiv = document.getElementById("loginFormDiv");
  const registerDiv = document.getElementById("registerFormDiv");

  // Hide the login form and show the register form
  loginDiv.classList.add("hidden");
  registerDiv.classList.remove("hidden");
}

function sendRequestForRegister() {
  const loginDiv = document.getElementById("loginFormDiv");
  const registerDiv = document.getElementById("registerFormDiv");

  // Hide the login form and show the register form
  loginDiv.classList.add("hidden");
  registerDiv.classList.remove("hidden");
}

document
  .getElementById("loginForm")
  .addEventListener("submit", function (event) {
    event.preventDefault(); // Prevent default form submission

    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    console.log(email);
    console.log(password);

    fetch("https://localhost:7078/api/auth/login", {
      method: "POST",
      body: JSON.stringify({
        id: 0,
        name: "test",
        email: email,
        password: password,
      }),
      headers: {
        "Content-Type": "application/json; charset=UTF-8",
      },
    })
      .then((response) => {
        if (response.status === 200) {
          return response.json(); // Parse the response JSON
        } else {
          throw new Error(`HTTP error! Status: ${response.status}`);
        }
      })
      .then((data) => {
        console.log("Response data:", data);

        // Store the token in local storage
        localStorage.setItem("authToken", data.token);
        console.log(data.token);

        // Redirect to another page
        window.location.href = "page.html";
      })
      .catch((error) => console.error("Error:", error));
  });

document
  .getElementById("registerForm")
  .addEventListener("submit", function (event) {
    event.preventDefault(); // Prevent default form submission

    const username = document.getElementById("usernameReg").value;
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    console.log(email);
    console.log(username);
    console.log(password);

    const fetchData = fetch("https://localhost:7078/api/auth/register", {
      method: "POST",
      body: JSON.stringify({
        id: 0,
        name: username,
        email: email,
        password: password,
      }),
      headers: {
        "Content-type": "application/json; charset=UTF-8",
      },
    });
    fetchData
      .then((response) => {
        if (response.status === 200) {
          window.location.href = "page.html";
        } else {
          console.log(response.status);
        }
        return response.json();
      })
      .then((json) => console.log(json))
      .catch((error) => console.error("Error:", error));
  });
