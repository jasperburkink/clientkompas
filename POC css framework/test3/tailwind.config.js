/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./dist/*.{html,js}"],
  theme: {
    extend: {
      colors: {
      'mainBlue' : '#148CB8',
      'mainGray' : '#E3E3E3',
      'mainLightGray' : '#F9F9F9',

      'subBlue1' : '#117DA3',
      'subBlue2' : '#0c6a8c',
      'subGray1' : '#CECECE',
      'subGray2' : '#B3B3B3'
      },
      width: {
        '50px' : '50px',
        '150px' : '150px',
        '200px' : '200px',
        'sidebarBlue' : '374px',
        'sidebarGray' : '474px',
        '50rem' : '50rem'
      },
      height: {
        '50px' : '50px',
        '65px' : '65px',
        '100px' : '100px',
        '400px' : '400px',
        '500px' : '500px',
        'sidebar' : '125vh'
      },
      margin: {
        '15px' : '15px',
        '50px' : '50px',
        '100px' : '100px',
        '150px' : '150px',
        '200px' : '200px',
        '250px' : '250px',
        'sidebarGray' : '430px',
        'line' : '185px',
        'darkLine' : '90px'
      },
      padding: {
        '50px' : '50px'
      },
      borderRadius: {
        "sidebarT" : '50px 100%',
        "sidebarB" : '200px 100%',
        "50%" : "50%"
      },
      rotate: {
        "line" : "50deg",
        "darkLine" : "30deg"
      }
    },
  },
  plugins: [],
}

