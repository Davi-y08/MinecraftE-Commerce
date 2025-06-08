import { useState } from "react";
import { useMutation } from "react-query";
import { useNavigate } from "react-router-dom";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const fetchData = async ({ email, password }: { email: string; password: string }) => {
    const response = await fetch("https://localhost:7253/api/v1/Login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        // NÃO enviar Authorization aqui, pois é o login
      },
      body: JSON.stringify({
        emailforlogin: email,
        passwordforlogin: password,
      }),
      credentials: "include",
    });

    const data = await response.json();

    if (!response.ok) {
      throw new Error(data.message || "Erro ao fazer login");
    }

    // Salva token e pfp no localStorage para uso futuro
    localStorage.setItem("token", data.token);
    localStorage.setItem("pfp", data.pfp);

    return data;
  };

  const { mutate, isLoading, isError, error } = useMutation(fetchData, {
    onSuccess: () => {
      navigate("/");
    },
  });

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    mutate({ email, password });
  };

  return (
    <div>
      <form onSubmit={handleSubmit}>
        <label htmlFor="email">Email: </label>
        <input
          onChange={(e) => setEmail(e.target.value)}
          id="email"
          type="email"
          value={email}
          required
        />
        <br />
        <br />
        <label htmlFor="password">Password: </label>
        <input
          onChange={(e) => setPassword(e.target.value)}
          type="password"
          value={password}
          required
        />
        <br />
        <button type="submit" disabled={isLoading}>
          {isLoading ? "Loading..." : "Send"}
        </button>
      </form>
      {isError && <p style={{ color: "red" }}>{(error as Error).message}</p>}
    </div>
  );
}

export default Login;
