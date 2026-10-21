import express from "express";
import cors from "cors";
import mysql from "mysql2/promise";
import dotenv from "dotenv";

dotenv.config();

const app = express();
app.use(cors());
app.use(express.json());

// KORREKT databaseforbindelse
const db = mysql.createPool({
  host: process.env.MYSQLHOST,
  user: process.env.MYSQLUSER,
  password: process.env.MYSQLPASSWORD,
  database: process.env.MYSQLDATABASE,
  port: process.env.MYSQLPORT,
  ssl: {
    rejectUnauthorized: false
  }
});

// Test route
app.get("/", (req, res) => {
  res.send("Backend virker!");
});

app.get("/user/search", async (req, res)=>{
  try{
    const name = req.query.name || "";

    const [rows] = await db.query(
      "SELECT id, name FROM User WHERE name LIKE ?",
      [`%${name}%`]
    );

    res.json(rows);
  }catch (error) {
    console.error(error);
    res.status(500).json({error: "Serverfejl"})

  }
});

app.get("/subjects", async (req, res) =>{
  try{
    const grade = req.query.grade;

    const[rows] = await db.query(
      "SELECT id, name FROM Subjects WHERE grade = ?",
      [grade]
    );

    res.json(rows);
  }catch (error){
    console.error(error); 
    res.status(500).json({ error: "Serverfejl" });  }
});

app.post("/selection", async (req, res) => {
  try{
    const {userId, class: grade, subjectId} = req.body;

    await db.query(
      "INSERT INTO Selection (user_id, class, subject_id) VALUES (?, ?, ?)",
      [userId, grade, subjectId]
    );

    res.json({message: "Valgt gemt!"});
  }catch (error){
    console.error(error); 
    res.status(500).json({ error: "Serverfejl" });  }
});


// Opret bruger
app.post("/api/users", async (req, res) => {
  console.log("REQ BODY:", req.body);

  try {
    const { name, email, password, role } = req.body;

    const allowedUserTypes = [
      "teacher",
      "student",
      "admin",
      "staff",
      "guest",
      "headmaster"
    ];

    if (!name || !email || !password || !role) {
      return res.status(400).json({ error: "Manglende felter" });
    }

    if (!allowedUserTypes.includes(role)) {
      return res.status(400).json({ error: "Ugyldig brugertype" });
    }

    const sql =
      "INSERT INTO User (name, email, password, role) VALUES (?, ?, ?, ?)";
    await db.query(sql, [name, email, password, role]);

    return res.status(201).json({ message: "Bruger oprettet" });
  } catch (error) {
    console.error(error);
    return res.status(500).json({ message: "Server fejl" });
  }
});

app.listen(process.env.PORT || 5000, () => {
  console.log("Server kører på port", process.env.PORT || 5000);
});

//tildele en bruger bestemte klasse
app.get("/api/users/:id/classes", async (req, res) => {
  try{
    const userId = req.params.id;

    const [rows] = await db.query(
      "SELECT DISTINCT class FROM Selection WHERE  user_id = ?",
      [userId]
    );

    const classes = rows.map(r => r.class);

    res.json(classes);
  } catch (error){
    console.error(error);
    res.status(500).json({ error: "Servcerfejl"});
  }

});
